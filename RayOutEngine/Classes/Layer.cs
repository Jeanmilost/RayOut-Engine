/****************************************************************************
 * ==> Layer ---------------------------------------------------------------*
 ****************************************************************************
 * Description : Transparency layer for rendering                           *
 * Developer   : Jean-Milost Reymond                                        *
 ****************************************************************************
 * MIT License - RayOut Engine                                              *
 *                                                                          *
 * Permission is hereby granted, free of charge, to any person obtaining a  *
 * copy of this software and associated documentation files (the            *
 * "Software"), to deal in the Software without restriction, including      *
 * without limitation the rights to use, copy, modify, merge, publish,      *
 * distribute, sublicense, and/or sell copies of the Software, and to       *
 * permit persons to whom the Software is furnished to do so, subject to    *
 * the following conditions:                                                *
 *                                                                          *
 * The above copyright notice and this permission notice shall be included  *
 * in all copies or substantial portions of the Software.                   *
 *                                                                          *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS  *
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF               *
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.   *
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY     *
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,     *
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE        *
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                   *
 ****************************************************************************/

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RayOutEngine.Classes
{
    /**
    * Transparency layer for rendering
    *@author Jean-Milost Reymond
    */
    class Layer
    {
        private Rectangle m_ClientRect = new Rectangle();

        /**
        * Gets or sets the boundaries
        */
        public Boundaries Boundaries { get; set; }  = new Boundaries();

        /**
        * Gets or sets the ray sources
        */
        public List<RaySource> RaySources { get; set; } = new List<RaySource>();

        /**
        * Gets or sets the client rectangle
        */
        public Rectangle ClientRect
        {
            get
            {
                return m_ClientRect;
            }

            set
            {
                m_ClientRect = value;

                float width  = value.Width  - 1.0f;
                float height = value.Height - 1.0f;

                Boundaries[0].Limit = new Line2(new Vector2(),              new Vector2(width, 0.0f));
                Boundaries[1].Limit = new Line2(new Vector2(width, 0.0f),   new Vector2(width, height));
                Boundaries[2].Limit = new Line2(new Vector2(0.0f,  height), new Vector2(width, height));
                Boundaries[3].Limit = new Line2(new Vector2(),              new Vector2(0.0f,  height));
            }
        }

        /**
        * Constructor
        */
        public Layer()
        {
            // add 4 boundaries for the client limits
            Boundaries.Add(new Vector2(), new Vector2());
            Boundaries.Add(new Vector2(), new Vector2());
            Boundaries.Add(new Vector2(), new Vector2());
            Boundaries.Add(new Vector2(), new Vector2());
        }

        /**
        * Constructor
        *@param clientRect - client rectangle
        */
        public Layer(Rectangle clientRect)
        {
            ClientRect = clientRect;
        }

        public void Draw(Graphics gfx, Vector2 pos)
        {
            gfx.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (RaySource raySource in RaySources)
            {
                // Create a path that consists of a single ellipse
                GraphicsPath rayGradientPath = new GraphicsPath();
                rayGradientPath.AddEllipse((int)pos.X - 250, (int)pos.Y - 250, 500, 500);

                // use the path to construct a brush
                PathGradientBrush rayGradientBrush = new PathGradientBrush(rayGradientPath);

                // set the color at the center of the path
                rayGradientBrush.CenterColor = Color.FromArgb(0, 0, 0, 0);

                // set the color along the entire boundary of the path
                Color[] surroundColors = { Color.FromArgb(192, 0, 0, 0) };
                rayGradientBrush.SurroundColors = surroundColors;

                /*REM
                Color[] colors =
                {
                    Color.FromArgb(255, 0, 0, 0),
                    Color.FromArgb(255, 0, 0, 0),
                    Color.FromArgb(0, 0, 0, 0)
                };

                float[] colorPos =
                {
                    0.0f,
                    0.001f,
                    1.0f
                };

                ColorBlend colorBlend = new ColorBlend(3);
                colorBlend.Colors     = colors;
                colorBlend.Positions  = colorPos;

                rayGradientBrush.InterpolationColors = colorBlend;
                */

                /*
                raySource.Pos    = pos;
                raySource.RayPen = new Pen(rayGradientBrush);
                raySource.Draw(gfx, Boundaries, false);
                */

                /*
                GraphicsPath rayPath = new GraphicsPath();
                raySource.Pos = pos;
                raySource.FillPath(rayPath, Boundaries, false, false);
                gfx.DrawPath(new Pen(rayGradientBrush), rayPath);
                */

                /**/
                GraphicsPath rayPath = new GraphicsPath();
                raySource.Pos = pos;
                raySource.FillPath(rayPath, Boundaries, true, false);
                gfx.FillPath(rayGradientBrush, rayPath);
                /**/

                /*
                gfx.SetClip(m_ClientRect);
                gfx.SetClip(rayPath, CombineMode.Exclude);
                SolidBrush blackBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
                gfx.FillRectangle(blackBrush, m_ClientRect);
                gfx.ResetClip();

                gfx.SetClip(m_ClientRect);
                gfx.SetClip(rayGradientPath, CombineMode.Exclude);
                gfx.FillRectangle(blackBrush, m_ClientRect);
                gfx.ResetClip();
                */

                gfx.SetClip(m_ClientRect);
                gfx.SetClip(rayPath, CombineMode.Exclude);
                gfx.SetClip(rayGradientPath, CombineMode.Complement);
                gfx.SetClip(m_ClientRect, CombineMode.Xor);
                SolidBrush blackBrush = new SolidBrush(Color.FromArgb(192, 0, 0, 0));
                gfx.FillRectangle(blackBrush, m_ClientRect);
                gfx.ResetClip();
            }

            Boundaries.Draw(gfx);
        }
    }
}
