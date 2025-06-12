# WindowsOperationsBroken

In May 2025 users of OneCommander and some other file managers started reporting that Copy/Move dialogs show only the first time operation is used. 

Affected
- Only some Windows 11 users
- Uninstalling KB5060842 "Security update" fixed the issue (need confirmation)
- Uninstalling KB5063060 also fixes it (different PC had this update instead of the one above)


Users reported
- No new software was installed
- No 3rd party window stylers, task managers, start menu replacements, or window utilities 

Developer
- Cannot reproduce on own PCs (even though one has the same Windows updates as one of the users who reported this)
- There was no update to OneCommander when this started (and years-old versions of OneCommander would also experience the same issue)
- OneCommander is using Explorer dialogs (IFileOperation)

Testing
- Tried setting owner to null, IntPtr.Zero, hwnd pointer to WPF window, and also wrapping WPF window into System.Windows.Forms.IWin32Window and passing as owner
- Regular STA thread and STA thread with dispatcher message pump as suggested by ChatGPT

What works
- Adding operation flag FOFX_NOMINIMIZEBOX makes the operation progress dialogs work again (According to 2 users previously reporting the issue) BUT this forces these operation dialog to look like Windows 7 (or Vista) dialogs and conflict prompts are also from that time. That is not the desirable "solution"

Reproducing
- This is minimal code to reproduce the issue using WPF and .NET8 (OneCommander is WPF and .NET 4.8 Framework)
- It is not exact code used in OneCommander as I hoped that butchering code from other's implementation will maybe uncover something I was missing in my own implementation of IFileOperations

Video demonistration: Run (good), Install update, Run (broken), Uninstall update, Run (fine again)
https://www.youtube.com/watch?v=r4fq6T4JrM4

Other pages mentioning issue
https://learn.microsoft.com/en-us/answers/questions/2278603/bug-with-file-operations-after-latest-windows-11-u
