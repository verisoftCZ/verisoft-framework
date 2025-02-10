
export function downloadText(
    filename: string,
    text: string,
    mimeType: string | undefined = 'text/plain'
  ): void {
    const blob = new Blob([text], { type: mimeType });
    downloadFile(filename, blob);
  }

export function downloadFile(
    filename: string,
    blob: Blob
  ): void {
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();
    window.URL.revokeObjectURL(url);
  }