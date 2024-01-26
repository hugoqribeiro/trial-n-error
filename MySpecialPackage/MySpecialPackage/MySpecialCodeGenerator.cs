using Microsoft.VisualStudio.TextTemplating.VSHost;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MySpecialPackage
{
    [Guid("81CBE30D-DDA6-4B9D-BEFB-0B3841C1B92B")]
    public partial class MySpecialCodeGenerator : BaseCodeGeneratorWithSite
    {
        private class TextTemplatingCallback : ITextTemplatingCallback
        {
            private Encoding outputEncoding = Encoding.UTF8;

            public bool HasErrors
            {
                get;
                private set;
            }

            public string FileExtension
            {
                get;
                private set;
            }

            public Encoding OutputEncoding => this.outputEncoding;

            public void ErrorCallback(bool warning, string message, int line, int column)
            {
                // NOTES:
                // This never gets called

                this.HasErrors = true;
            }

            public void SetFileExtension(string extension)
            {
                // NOTES:
                // This gets called

                this.FileExtension = extension;
            }

            public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
            {
                // NOTES:
                // This never gets called

                this.outputEncoding = encoding;
            }
        }

        // NOTES:
        // Microsoft.VisualStudio.TextTemplating.VSHost.dll is being loaded from
        // C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\IDE\PublicAssemblies
        // with file version 17.0.34518.88.

        private ITextTemplating VsTextTemplatingService => this.GetService<STextTemplating, ITextTemplating>();

        public override string GetDefaultExtension()
        {
            return ".cs";
        }

        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            // NOTES:
            // This was supposed to clean any error in the errors list from a previous transformation session.
            // It doesn't. Looking at the implementation of TextTemplatingService (in Microsoft.VisualStudio.TextTemplating.VSHost.dll)
            // it does call anything on member errorLogger to do anything like that.

            this.VsTextTemplatingService.BeginErrorSession();

            // NOTES:
            // In our scenario, this callback is used, among other things, to known if the text template
            // has any error. But ErrorCallback() is never called when there are errors in the text template.

            TextTemplatingCallback callback = new TextTemplatingCallback();

            string outputFileContent = this.VsTextTemplatingService.ProcessTemplate(inputFileName, inputFileContent, callback);

            this.VsTextTemplatingService.EndErrorSession();

            // NOTES:
            // HasErrors is always false...

            if (callback.HasErrors)
            {
                MessageBox.Show("Text transformation completed WITH errors.", "Code Gen", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Text transformation completed WITHOUT errors.", "Code Gen", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return callback.OutputEncoding.GetBytes(outputFileContent);
        }

        private TService GetService<TReg, TService>()
        {
            return (TService)this.GetService(typeof(TReg));
        }
    }
}
