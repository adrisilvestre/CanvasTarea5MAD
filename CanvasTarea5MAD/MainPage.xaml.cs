using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CanvasTarea5MAD
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        private Dictionary<long, SKPath> TemporaryPaths = new Dictionary<long, SKPath>();
        private List<SKPath> Paths = new List<SKPath>();
       
        private SKPaint TouchPathStrokeSelected = new SKPaint  //default SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Purple,
            StrokeWidth = 5
        };
        public MainPage()
        {
            InitializeComponent();

        }

        private void OnPainting(object sender, SKPaintSurfaceEventArgs e)
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear(SKColors.White);

                var textPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = SKColors.Orange,
                TextSize = 80
            };

         
            canvas.DrawText("AdriSharp", 60, 160, textPaint);
            SKPaint TouchPathStroke = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                Color = TouchPathStrokeSelected.Color,
                StrokeWidth = 5
            };


            foreach (var touchPath in TemporaryPaths)
            {
                canvas.DrawPath(touchPath.Value, TouchPathStroke);
            }
            foreach (var touchPath in Paths)
            {
                canvas.DrawPath(touchPath, TouchPathStroke);
            }
        }

       
        private void OnTouch(SKCanvasView sender, SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    var p = new SKPath();
                    p.MoveTo(e.Location);
                    TemporaryPaths[e.Id] = p;
                    break;
                case SKTouchAction.Moved:
                    if (e.InContact)
                        TemporaryPaths[e.Id].LineTo(e.Location);
                    break;
                case SKTouchAction.Released:
                    Paths.Add(TemporaryPaths[e.Id]);
                    TemporaryPaths.Remove(e.Id);
                    break;
            }

            e.Handled = true;

            // update the UI on the screen
            ((SKCanvasView)sender).InvalidateSurface();
        }

        private void OnClearButtonClicked(object sender, EventArgs args)
        { 
            Paths.Clear();
            TemporaryPaths.Clear();
            PaintSample.InvalidateSurface();
           
        }
        private void ColorSelection(object obj, EventArgs args)
        {
            var button = (Button)obj;
            TouchPathStrokeSelected.Color = button.BackgroundColor.ToSKColor();
            PaintSample.InvalidateSurface();

        }


    }
}
