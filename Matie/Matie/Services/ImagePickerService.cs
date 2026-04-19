using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace Matie.Services;

public sealed class ImagePickerService : IImagePickerService
{
    private const long maxBytes = 4 * 1024 * 1024;

    public async Task<byte[]?> PickImageBytesAsync(CancellationToken cancellationToken = default)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop
            || desktop.MainWindow is null)
        {
            return null;
        }

        var files = await desktop.MainWindow.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = "Выберите изображение",
                AllowMultiple = false,
                FileTypeFilter =
                [
                    new FilePickerFileType("Изображения")
                    {
                        Patterns = ["*.png", "*.jpg", "*.jpeg", "*.webp"]
                    }
                ]
            });

        if (files.Count == 0)
        {
            return null;
        }

        await using var stream = await files[0].OpenReadAsync();
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms, cancellationToken);
        if (ms.Length == 0)
        {
            return null;
        }

        if (ms.Length > maxBytes)
        {
            throw new InvalidOperationException("Файл больше 4 МБ. Выберите другое изображение.");
        }

        return ms.ToArray();
    }
}
