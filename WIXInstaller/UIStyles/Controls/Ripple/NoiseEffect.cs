﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace WIXInstaller.UIStyles.Controls.Ripple
{
    public class NoiseEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(NoiseEffect), 0);
        public static readonly DependencyProperty RandomInputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("RandomInput", typeof(NoiseEffect), 1);
        public static readonly DependencyProperty RatioProperty = DependencyProperty.Register("Ratio", typeof(double), typeof(NoiseEffect), new UIPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(0)));
        public NoiseEffect()
        {
            PixelShader pixelShader = new PixelShader();
            //pixelShader.UriSource =
              //  new Uri("/Bootstrapper;component/Ripple/Noise.ps", UriKind.Relative);
            this.PixelShader = pixelShader;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            //bitmap.UriSource = new Uri("pack://application:,,,/Bootstrapper;component/Ripple/Images/noise.png");
            bitmap.EndInit();
            this.RandomInput =
                new ImageBrush(bitmap)
                {
                    TileMode = TileMode.Tile,
                    Viewport = new Rect(0, 0, 800, 600),
                    ViewportUnits = BrushMappingMode.Absolute
                };

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(RandomInputProperty);
            this.UpdateShaderValue(RatioProperty);
        }
        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        /// <summary>The second input texture.</summary>
        public Brush RandomInput
        {
            get
            {
                return ((Brush)(this.GetValue(RandomInputProperty)));
            }
            set
            {
                this.SetValue(RandomInputProperty, value);
            }
        }
        public double Ratio
        {
            get
            {
                return ((double)(this.GetValue(RatioProperty)));
            }
            set
            {
                this.SetValue(RatioProperty, value);
            }
        }
    }
}