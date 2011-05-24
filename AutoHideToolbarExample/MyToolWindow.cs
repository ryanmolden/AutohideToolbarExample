using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace MicrosoftIT.AutoHideToolbarExample
{
    [Guid("54577230-b515-4067-b071-bb0be00f2a9c")]
    public class MyToolWindow : ToolWindowPane, IVsSelectionEvents
    {
        private uint selectionMonCookie = VSConstants.VSCOOKIE_NIL;
        private uint cmdUIContextCookie = VSConstants.VSCOOKIE_NIL;

        public MyToolWindow()
        {
            this.Caption = Resources.ToolWindowTitle;
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;
            
            base.Content = new MyControl();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.selectionMonCookie != VSConstants.VSCOOKIE_NIL) 
            {
                IVsMonitorSelection selMon = (IVsMonitorSelection)GetService(typeof(SVsShellMonitorSelection));
                ErrorHandler.ThrowOnFailure(selMon.UnadviseSelectionEvents(this.selectionMonCookie));
                this.selectionMonCookie = VSConstants.VSCOOKIE_NIL;
            }

            base.Dispose(disposing);
        }

        protected override void Initialize()
        {
            base.Initialize();

            IVsMonitorSelection selMon = (IVsMonitorSelection)GetService(typeof(SVsShellMonitorSelection));
            ErrorHandler.ThrowOnFailure(selMon.AdviseSelectionEvents(this, out selectionMonCookie));

            Guid cmdGuid = new Guid(GuidList.guidAutoHideToolbarExampleCmdSetString);
            ErrorHandler.ThrowOnFailure(selMon.GetCmdUIContextCookie(ref cmdGuid, out this.cmdUIContextCookie));
        }

        public int OnCmdUIContextChanged(uint dwCmdUICookie, int fActive)
        {
            return VSConstants.S_OK;
        }

        public int OnElementValueChanged(uint elementid, object varValueOld, object varValueNew)
        {
            //Watch the active toolwindow in the shell, if it becomes us or was us then toggle our command UI context
            //to be active (if we are becoming the selected toolwindow) or inactive (if we are losing our status
            //as the selected toolwindow).
            if ((VSConstants.VSSELELEMID)elementid == VSConstants.VSSELELEMID.SEID_WindowFrame)
            {
                if (varValueOld == this.Frame || varValueNew == this.Frame)
                {
                    IVsMonitorSelection selMon = (IVsMonitorSelection)GetService(typeof(SVsShellMonitorSelection));

                    int isActive = ((varValueNew == this.Frame) ? 1 : 0);
                    ErrorHandler.ThrowOnFailure(selMon.SetCmdUIContext(this.cmdUIContextCookie, isActive));
                }
            }

            return VSConstants.S_OK;
        }

        public int OnSelectionChanged(IVsHierarchy pHierOld,
                                      uint itemidOld,
                                      IVsMultiItemSelect pMISOld,
                                      ISelectionContainer pSCOld,
                                      IVsHierarchy pHierNew,
                                      uint itemidNew,
                                      IVsMultiItemSelect pMISNew,
                                      ISelectionContainer pSCNew)
        {
            return VSConstants.S_OK;
        }
    }
}
