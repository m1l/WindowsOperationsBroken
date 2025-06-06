using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace WindowsOperationsBroken
{
    [ComImport]
    [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItem
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        object BindToHandler(IBindCtx pbc, ref Guid bhid, ref Guid riid);

        IShellItem GetParent();

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetDisplayName(SIGDN sigdnName);

        uint GetAttributes(uint sfgaoMask);

        int Compare(IShellItem psi, uint hint);
    }

    [Flags]
    public enum SIGDN : uint
    {
        SIGDN_NORMALDISPLAY = 0x00000000,
        SIGDN_PARENTRELATIVEPARSING = 0x80018001,
        SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
        SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
        SIGDN_PARENTRELATIVEEDITING = 0x80031001,
        SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
        SIGDN_FILESYSPATH = 0x80058000,
        SIGDN_URL = 0x80068000
    }
    [Flags]
    internal enum FileOperationFlags : uint
    {
        FOF_MULTIDESTFILES = 0x0001,
        FOF_CONFIRMMOUSE = 0x0002,
        FOF_SILENT = 0x0004,  // don't create progress/report
        FOF_RENAMEONCOLLISION = 0x0008,
        FOF_NOCONFIRMATION = 0x0010,  // Don't prompt the user.
        FOF_WANTMAPPINGHANDLE = 0x0020,  // Fill in SHFILEOPSTRUCT.hNameMappings
                                         // Must be freed using SHFreeNameMappings
        FOF_ALLOWUNDO = 0x0040,  // Recycle bin
        FOF_FILESONLY = 0x0080,  // on *.*, do only files
        FOF_SIMPLEPROGRESS = 0x0100,  // means don't show names of files
        FOF_NOCONFIRMMKDIR = 0x0200,  // don't confirm making any needed dirs
        FOF_NOERRORUI = 0x0400,  // don't put up error UI
        FOF_NOCOPYSECURITYATTRIBS = 0x0800,  // dont copy NT file Security Attributes
        FOF_NORECURSION = 0x1000,  // don't recurse into directories.
        FOF_NO_CONNECTED_ELEMENTS = 0x2000,  // don't operate on connected file elements.
        FOF_WANTNUKEWARNING = 0x4000,  // during delete operation, warn if nuking instead of recycling (partially overrides FOF_NOCONFIRMATION)
        FOF_NORECURSEREPARSE = 0x8000,  // treat reparse points as objects, not containers

        FOFX_NOSKIPJUNCTIONS = 0x00010000,  // Don't avoid binding to junctions (like Task folder, Recycle-Bin)
        FOFX_PREFERHARDLINK = 0x00020000,  // Create hard link if possible
        FOFX_SHOWELEVATIONPROMPT = 0x00040000,  // Show elevation prompts when error UI is disabled (use with FOF_NOERRORUI)
        FOFX_EARLYFAILURE = 0x00100000,  // Fail operation as soon as a single error occurs rather than trying to process other items (applies only when using FOF_NOERRORUI)
        FOFX_PRESERVEFILEEXTENSIONS = 0x00200000,  // Rename collisions preserve file extns (use with FOF_RENAMEONCOLLISION)
        FOFX_KEEPNEWERFILE = 0x00400000,  // Keep newer file on naming conflicts
        FOFX_NOCOPYHOOKS = 0x00800000,  // Don't use copy hooks
        FOFX_NOMINIMIZEBOX = 0x01000000,  // Don't allow minimizing the progress dialog
        FOFX_MOVEACLSACROSSVOLUMES = 0x02000000,  // Copy security information when performing a cross-volume move operation
        FOFX_DONTDISPLAYSOURCEPATH = 0x04000000,  // Don't display the path of source file in progress dialog
        FOFX_DONTDISPLAYDESTPATH = 0x08000000,  // Don't display the path of destination file in progress dialog
        FOFX_RECYCLEONDELETE = 0x00080000,   // Introduced in Windows 8. When a file is deleted, send it to the Recycle Bin rather than permanently deleting it.

        //https://github.com/dahall/Vanara/blob/1fb8a2dc8a116995831730a7d433a07f940cc5ef/PInvoke/Shell32/ShObjIdl.IFileOperation.cs
        FOFX_ADDUNDORECORD = 0x20000000,            /// Introduced in Windows 8. The file operation was user-invoked and should be placed on the undo stack. This flag is preferred to FOF_ALLOWUNDO
        FOFX_COPYASDOWNLOAD = 0x40000000,           /// Introduced in Windows 7. Display a Downloading instead of Copying message in the progress dialog.</summary>
        FOFX_DONTDISPLAYLOCATIONS = 0x80000000, 			/// <summary>Introduced in Windows 7. Do not display the location line in the progress dialog.</summary>
    }
}
