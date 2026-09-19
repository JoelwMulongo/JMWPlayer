using System.Windows;
using System.Windows.Input;
using LibVLCSharp.Shared;
using Microsoft.Win32;

namespace JMWPlayer;

public partial class MainWindow : Window
{
    private readonly LibVLC _libVLC;
    private readonly MediaPlayer _player;
    private bool _isSeeking;
    private bool _isFullscreen;

    public MainWindow()
    {
        InitializeComponent();

        Core.Initialize();
        _libVLC = new LibVLC();
        _player = new MediaPlayer(_libVLC) { Volume = 80 };
        VideoView.MediaPlayer = _player;

        // These events fire on a background thread, so hop to the UI thread.
        _player.TimeChanged += (_, e) => Dispatcher.InvokeAsync(() => UpdateTime(e.Time));
        _player.LengthChanged += (_, e) => Dispatcher.InvokeAsync(() => TotalText.Text = Format(e.Length));

        Closing += (_, _) =>
        {
            _player.Stop();
            _player.Dispose();
            _libVLC.Dispose();
        };
    }

    // ---------- Opening files ----------
    private void Open_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog
        {
            Filter = "Media files|*.mp4;*.mkv;*.avi;*.mov;*.webm;*.mp3;*.flac;*.wav;*.m4a|All files|*.*"
        };
        if (dlg.ShowDialog() == true) Play(dlg.FileName);
    }

    private void Window_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
            Play(files[0]);
    }

    private void Play(string path)
    {
        using var media = new Media(_libVLC, path, FromType.FromPath);
        _player.Play(media);
        Title = $"JMWPlayer - {System.IO.Path.GetFileName(path)}";
    }

    // ---------- Play/Pause and volume ----------
    private void PlayPause_Click(object sender, RoutedEventArgs e) => _player.Pause();

    private void Volume_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_player != null) _player.Volume = (int)e.NewValue;
    }

    // ---------- Seek bar ----------
    private void UpdateTime(long timeMs)
    {
        CurrentText.Text = Format(timeMs);
        if (!_isSeeking) Seek.Value = _player.Position;   // Position is 0.0 to 1.0
    }

    private void Seek_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        => _isSeeking = true;

    private void Seek_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
    {
        _player.Position = (float)Seek.Value;
        _isSeeking = false;
    }

    private static string Format(long ms)
    {
        var t = TimeSpan.FromMilliseconds(ms);
        return t.TotalHours >= 1 ? t.ToString(@"h\:mm\:ss") : t.ToString(@"m\:ss");
    }

    // ---------- Keyboard shortcuts ----------
    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Space: _player.Pause(); break;
            case Key.Left:  if (_player.IsSeekable) _player.Time -= 5000; break;
            case Key.Right: if (_player.IsSeekable) _player.Time += 5000; break;
            case Key.Up:    Volume.Value = Math.Min(100, Volume.Value + 5); break;
            case Key.Down:  Volume.Value = Math.Max(0, Volume.Value - 5); break;
            case Key.M:     _player.Mute = !_player.Mute; break;
            case Key.F:     ToggleFullscreen(); break;
            case Key.Escape: if (_isFullscreen) ToggleFullscreen(); break;
            default: return;
        }
        e.Handled = true;
    }

    // ---------- Fullscreen ----------
    private void ToggleFullscreen()
    {
        _isFullscreen = !_isFullscreen;
        WindowState = WindowState.Normal;   // reset first so the taskbar gets covered properly

        if (_isFullscreen)
        {
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            ControlBar.Visibility = Visibility.Collapsed;
        }
        else
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            ControlBar.Visibility = Visibility.Visible;
        }
    }
}