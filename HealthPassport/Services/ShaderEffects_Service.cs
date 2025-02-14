using HealthPassport.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthPassport.Services
{
    public class ShaderEffects_Service : IShaderEffects
    {
        public void ApplyBlurEffect(Window window, int radius)
        {
            System.Windows.Media.Effects.BlurEffect objBlur = new System.Windows.Media.Effects.BlurEffect();
            objBlur.Radius = radius;
            window.Effect = objBlur;
        }

        public void ClearEffect(Window window)
        {
            window.Effect = null;
        }
    }
}
