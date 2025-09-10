using System;
using System.Windows.Forms;

namespace NSMBe4
{
    public class NSMBeForm : Form
    {
        public NSMBeForm()
        {
            if (Environment.OSVersion.Platform.ToString() == "Unix")
                this.HandleCreated += (sender, ex) => WmClass.SetWmClass("NSMBe", this.Handle);
        }
    }
}
