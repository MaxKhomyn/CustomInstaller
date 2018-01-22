//using SevenZip;
using System.Windows;

namespace WIXInstaller.Services
{
    class SevenZipService
    {
        //public static void Extractor(string Path, string ExtractPath)
        //{
        //    using (var extr = new SevenZipExtractor(Path))
        //    {
        //        MessageBox.Show(Path);
        //        MessageBox.Show(ExtractPath);
        //        //extr.Extracting += DoExtractingEvent();
        //        extr.ExtractArchive(ExtractPath);
        //        //DoFinishEvent();
        //    }
        //}
        //public static void ExtractorAcync(string Path, string ExtractPath)
        //{
        //    using (var extr = new SevenZipExtractor(Path))
        //    {
        //        //extr.Extracting += DoExtractingEvent();
        //        //extr.ExtractionFinished += (s, e) => { DoFinishEvent(); extr.Dispose(); extr = null; };
        //        extr.BeginExtractArchive(ExtractPath);
        //    };
        //}

        //public static void Compressor(string CompresPath, string ArchiveName)
        //{
        //    var cmpr = new SevenZipCompressor();
            
        //    cmpr.CompressDirectory(CompresPath, ArchiveName);
        //    //DoFinishEvent();
        //    cmpr = null;
        //}
        //public static void CompressorAcync(string CompresPath, string ArchiveName)
        //{
        //    var cmpr = new SevenZipCompressor();

        //    //cmpr.CompressionFinished += (s, e) => { DoFinishEvent(); cmpr = null; }
        //    cmpr.BeginCompressDirectory(CompresPath, ArchiveName);
        //}
    }
}