using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using AviRecorder.Video.Compression;

namespace AviRecorder.Video.TgaSequences
{
    public class TgaSequenceRecorder : IDisposable
    {
        public const int MaxSupportedFrameRate = TgaSequenceConverter.MaxSupportedFrameRate;

        private const int StateRunning = 0;
        private const int StateStopping = 1;

        private FileSystemWatcher _tgaWatcher;
        private TgaSequenceConverter _tgaConverter;
        private string _aviDirectory;
        private ISynchronizeInvoke _synchronizingObject;

        private BlockingCollection<FileSystemEventArgs> _queue;
        private Task _task;

        private string _lastDetectedFrame;
        private int _state;

        public TgaSequenceRecorder()
        {
            _tgaWatcher = new FileSystemWatcher
            {
                Filter = "*.tga",
                IncludeSubdirectories = false
            };

            _tgaWatcher.Changed += TgaWatcher_Changed;
            _tgaWatcher.Error += TgaWatcher_Error;
            _tgaConverter = new TgaSequenceConverter();
        }

        public event FileSystemEventHandler MovieStarted;
        public event FileSystemEventHandler FrameProcessed;
        public event EventHandler MovieEnded;
        public event EventHandler BufferOverflow;
        public event ErrorActionHandler ErrorActionRequested;
        public event ErrorEventHandler Error;

        public string TgaDirectory
        {
            get => _tgaWatcher.Path;
            set
            {
                CheckIsRunning();
                _tgaWatcher.Path = value;
            }
        }

        public string AviDirectory
        {
            get => _aviDirectory;
            set
            {
                if (!Directory.Exists(value))
                    throw new ArgumentException("The directory \"" + value + "\" does not exist.", nameof(value));

                CheckIsRunning();
                _aviDirectory = value;
            }
        }

        public int FrameRate
        {
            get => _tgaConverter.FrameRate;
            set
            {
                if (value < 1 || value > TgaSequenceConverter.MaxSupportedFrameRate)
                    throw new ArgumentOutOfRangeException(nameof(value));

                CheckIsRunning();
                _tgaConverter.FrameRate = value;
            }
        }

        public int FrameBlendingFactor
        {
            get => _tgaConverter.FrameBlendingFactor;
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value));

                CheckIsRunning();
                _tgaConverter.FrameBlendingFactor = value;
            }
        }

        public int FramesToProcess
        {
            get => _tgaConverter.FramesToProcess;
            set
            {
                if (value < 1 || value > _tgaConverter.FrameBlendingFactor)
                    throw new ArgumentOutOfRangeException(nameof(value));

                CheckIsRunning();
                _tgaConverter.FramesToProcess = value;
            }
        }

        public bool DeleteOnClose
        {
            get => _tgaConverter.DeleteOnClose;
            set
            {
                CheckIsRunning();
                _tgaConverter.DeleteOnClose = value;
            }
        }

        public VideoCompressor Compressor
        {
            get => _tgaConverter.Compressor;
            set
            {
                if (value != null && value.IsCompressing)
                    throw new ArgumentException("Video compressor is already compressing.", nameof(value));

                CheckIsRunning();
                _tgaConverter.Compressor = value;
            }
        }

        public ISynchronizeInvoke SynchronizingObject
        {
            get => _synchronizingObject;
            set
            {
                CheckIsRunning();
                _synchronizingObject = value;
            }
        }

        public bool IsRunning => _task != null && !_task.IsCompleted;

        public Task Start()
        {
            if (!Directory.Exists(_tgaWatcher.Path))
                throw new DirectoryNotFoundException("TGA directory does not exist.");
            if (!Directory.Exists(_aviDirectory))
                throw new DirectoryNotFoundException("AVI directory does not exist.");
            if (IsRunning)
                throw new InvalidOperationException("Recording is already in progress.");

            _queue?.Dispose();
            _queue = new BlockingCollection<FileSystemEventArgs>(new ConcurrentQueue<FileSystemEventArgs>());

            try
            {
                _tgaWatcher.EnableRaisingEvents = true;
            }
            catch (Exception)
            {
                _queue.Dispose();
                throw;
            }

            _lastDetectedFrame = null;
            _state = StateRunning;

            return _task = Task.Run((Action)ProcessFrames);
        }

        public void Stop()
        {
            if (Interlocked.CompareExchange(ref _state, StateStopping, StateRunning) != StateRunning)
                return;

            _tgaWatcher.EnableRaisingEvents = false;
            _queue.CompleteAdding();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                return;

            if (IsRunning)
                throw new InvalidOperationException("Recorder may not be disposed while running.");

            _tgaWatcher.Dispose();
            _tgaConverter.Dispose();

            _queue?.Dispose();
            _task?.Dispose();

            MovieStarted = null;
            FrameProcessed = null;
            MovieEnded = null;
            BufferOverflow = null;
            ErrorActionRequested = null;
            Error = null;
        }

        protected virtual void OnMovieStarted(FileSystemEventArgs e)
        {
            var handler = MovieStarted;

            if (handler == null)
                return;
            
            if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
                SynchronizingObject.BeginInvoke(handler, new object[] { this, e });
            else
                handler(this, e);
        }

        protected virtual void OnFrameProcessed(FileSystemEventArgs e)
        {
            var handler = FrameProcessed;

            if (handler == null)
                return;
            
            if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
                SynchronizingObject.BeginInvoke(handler, new object[] { this, e });
            else
                handler(this, e);
        }

        protected virtual void OnMovieEnded()
        {
            var handler = MovieEnded;

            if (handler == null)
                return;
            
            if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
                SynchronizingObject.BeginInvoke(handler, new object[] { this, EventArgs.Empty });
            else
                handler(this, EventArgs.Empty);
        }

        protected virtual void OnBufferOverflow()
        {
            var handler = BufferOverflow;

            if (handler == null)
                return;
            
            if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
                SynchronizingObject.BeginInvoke(handler, new object[] { this, EventArgs.Empty });
            else
                handler(this, EventArgs.Empty);
        }

        protected virtual void OnErrorActionRequested(RetryErrorEventArgs e)
        {
            var errorActionHandler = ErrorActionRequested;

            if (errorActionHandler == null)
                return;
            
            if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
                SynchronizingObject.Invoke(errorActionHandler, new object[] { this, e });
            else
                errorActionHandler(this, e);
        }

        protected virtual void OnError(ErrorEventArgs e)
        {
            var errorHandler = Error;

            if (errorHandler == null)
                return;
            
            if (SynchronizingObject != null && SynchronizingObject.InvokeRequired)
                SynchronizingObject.BeginInvoke(errorHandler, new object[] { this, e });
            else
                errorHandler(this, e);
        }

        private void CheckIsRunning()
        {
            if (IsRunning)
                throw new InvalidOperationException("Settings must not be changed while a recording is in progress.");
        }

        private void ProcessFrames()
        {
            try
            {
                TgaSequence sequence = null;

                while (!_queue.IsCompleted)
                {
                    if (!_queue.TryTake(out var e, Timeout.Infinite))
                        continue;

                    if (sequence != null)
                    {
                        if (sequence.IsNextFrame(e.Name))
                        {
                            ProcessFrame(e);
                            sequence.CurrentFrame++;
                            continue;
                        }

                        EndMovie();
                    }

                    if ((sequence = TgaSequence.FromPath(e.FullPath)) != null)
                        StartMovie(e, sequence);
                }

                if (sequence != null)
                    EndMovie();
            }
            catch (Exception ex) when (ex is IOException ||
                                       ex is UnauthorizedAccessException ||
                                       ex is SecurityException ||
                                       ex is VideoCompressorException ||
                                       ex is RecoverableException)
            {
                Stop();

                OnError(new ErrorEventArgs(ex));
            }
        }

        private void StartMovie(FileSystemEventArgs tgaPath, TgaSequence sequence)
        {
            var aviPath = GetMoviePath(sequence);
            var wavPath = sequence.WavPath;

            while (true)
            {
                try
                {
                    _tgaConverter.StartMovie(tgaPath.FullPath, File.Exists(wavPath) ? wavPath : null, aviPath.FullPath);
                    break;
                }
                catch (RecoverableException ex)
                {
                    var retryErrorEventArgs = new RetryErrorEventArgs(ex);
                    OnErrorActionRequested(retryErrorEventArgs);

                    if (!retryErrorEventArgs.Retry)
                        throw;
                }
            }

            OnMovieStarted(aviPath);
            OnFrameProcessed(tgaPath);
        }

        private FileSystemEventArgs GetMoviePath(TgaSequence sequence)
        {
            var path = $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {sequence.Name}.avi";

            return new FileSystemEventArgs(WatcherChangeTypes.Created, _aviDirectory, path);
        }

        private void ProcessFrame(FileSystemEventArgs tgaPath)
        {
            while (true)
            {
                try
                {
                    _tgaConverter.ProcessFrame(tgaPath.FullPath);
                    break;
                }
                catch (RecoverableException ex)
                {
                    var retryErrorEventArgs = new RetryErrorEventArgs(ex);
                    OnErrorActionRequested(retryErrorEventArgs);

                    if (!retryErrorEventArgs.Retry)
                        throw;
                }
            }

            while (true)
            {
                try
                {
                    _tgaConverter.ProcessAudio();
                    break;
                }
                catch (RecoverableException ex)
                {
                    var retryErrorEventArgs = new RetryErrorEventArgs(ex);
                    OnErrorActionRequested(retryErrorEventArgs);

                    if (!retryErrorEventArgs.Retry)
                        throw;
                }
            }

            OnFrameProcessed(tgaPath);
        }

        private void EndMovie()
        {
            while (true)
            {
                try
                {
                    _tgaConverter.ProcessAllAudio();
                    break;
                }
                catch (RecoverableException ex)
                {
                    var retryErrorEventArgs = new RetryErrorEventArgs(ex);
                    OnErrorActionRequested(retryErrorEventArgs);

                    if (!retryErrorEventArgs.Retry)
                        throw;
                }
            }

            while (true)
            {
                try
                {
                    _tgaConverter.EndMovie();
                    break;
                }
                catch (RecoverableException ex)
                {
                    var retryErrorEventArgs = new RetryErrorEventArgs(ex);
                    OnErrorActionRequested(retryErrorEventArgs);

                    if (!retryErrorEventArgs.Retry)
                        throw;
                }
            }

            OnMovieEnded();
        }

        private void TgaWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (string.Equals(e.Name, _lastDetectedFrame, StringComparison.OrdinalIgnoreCase))
                return;

            try
            {
                _queue.Add(e);
                _lastDetectedFrame = e.Name;
            }
            catch (InvalidOperationException)
            {
            }
        }

        private void TgaWatcher_Error(object sender, ErrorEventArgs e)
        {
            var ex = e.GetException();

            if (ex.GetType() == typeof(InternalBufferOverflowException))
            {
                OnBufferOverflow();
                return;
            }

            _queue.CompleteAdding();
            OnError(e);
        }
    }
}
