using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

public class PageCustom : System.Web.UI.Page
{

    #region Logging

    public global::Prototype.Providers.Logging Logging;
    public event global::Prototype.Providers.EventHandler EventResulted;

    public void RaiseLogging()
    {
        this.Logging.Raise(EventResulted);
    }

    public PageCustom()
    {
        this.EventResulted += new Prototype.Providers.EventHandler(_Object_EventResulted);
    }

    public void PlugEventResult(dynamic _objectEventResulted)
    {
        _objectEventResulted.EventResulted += new Prototype.Providers.EventHandler(_Object_EventResulted);
    }

    private void _Object_EventResulted(object sender, Prototype.Providers.EventArgsCustom e)
    {
        Page.ShowEventLogging(e);
    }

    #endregion

    #region ViewState Compression

    protected override void OnInit(EventArgs e)
    {
        ViewStateCompression.StateCompression = Deflater.BEST_COMPRESSION;
        base.OnInit(e);
    }

    protected override void SavePageStateToPersistenceMedium(Object state)
    {
        if (ViewStateCompression.StateCompression == Deflater.NO_COMPRESSION)
        {
            base.SavePageStateToPersistenceMedium(state);
            return;
        }

        Object viewState = state;
        if (state is Pair)
        {
            Pair statePair = (Pair)state;
            PageStatePersister.ControlState = statePair.First;
            viewState = statePair.Second;
        }

        using (StringWriter writer = new StringWriter())
        {
            new LosFormatter().Serialize(writer, viewState);
            string base64 = writer.ToString();
            byte[] compressed = ViewStateCompression.Compress(Convert.FromBase64String((base64)));
            PageStatePersister.ViewState = Convert.ToBase64String(compressed);
        }
        PageStatePersister.Save();
    }

    protected override Object LoadPageStateFromPersistenceMedium()
    {
        if (ViewStateCompression.StateCompression == Deflater.NO_COMPRESSION)
            return base.LoadPageStateFromPersistenceMedium();

        PageStatePersister.Load();
        String base64 = PageStatePersister.ViewState.ToString();
        byte[] state = ViewStateCompression.Decompress(Convert.FromBase64String(base64));
        string serializedState = Convert.ToBase64String(state);

        object viewState = new LosFormatter().Deserialize(serializedState);
        return new Pair(PageStatePersister.ControlState, viewState);
    }

    #endregion
}

public class UControlCustom : System.Web.UI.UserControl
{

    #region Logging

    public global::Prototype.Providers.Logging Logging;
    public event global::Prototype.Providers.EventHandler EventResulted;

    public delegate object GetObjectHandler();

    public void RaiseLogging()
    {
        this.Logging.Raise(EventResulted);
    }

    public UControlCustom()
    {
        this.EventResulted += new Prototype.Providers.EventHandler(_Object_EventResulted);
    }

    public void PlugEventResult(dynamic _objectEventResulted)
    {
        _objectEventResulted.EventResulted += new Prototype.Providers.EventHandler(_Object_EventResulted);
    }

    private void _Object_EventResulted(object sender, Prototype.Providers.EventArgsCustom e)
    {
        Page.ShowEventLogging(e);
    }

    #endregion
}