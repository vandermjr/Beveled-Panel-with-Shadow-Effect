using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BevelPanel
{
    public class Block : Panel
    {
        #region Variables

        Color _startColor = Color.DimGray;
        Color _endColor = Color.DarkGray;
        private Color _borderColor = Color.Red;

        private int _rectRadius = 5;
        private int _shadowShift = 0;
        private int _edgeWidth = 2;

        private Color edgeColor1;
        private Color edgeColor2;

        /// <summary>
        /// The width of an edge
        /// </summary>
        [Browsable(true), Category("AdvancedPanel"), Description("The width of an edge.")]
        public int EdgeWidth
        {
            get => _edgeWidth;
            set
            {
                _edgeWidth = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets begin gradient color
        /// </summary>
        [Browsable(true), Category("AdvancedPanel"), Description("The begin gradient color.")]
        public Color StartColor
        {
            get => _startColor;
            set
            {
                _startColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets end gradient color
        /// </summary>
        [Browsable(true), Category("AdvancedPanel"), Description("The end gradient color.")]
        public Color EndColor
        {
            get => _endColor;
            set
            {
                _endColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the shadow shift
        /// </summary>
        [Browsable(true), Category("AdvancedPanel"), Description("The shadow shift.")]
        public int ShadowShift
        {
            get => _shadowShift;
            set
            {
                _shadowShift = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Border color in flat mode
        /// </summary>
        [Browsable(true), Category("AdvancedPanel"), Description("The flat border color.")]
        public Color FlatBorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the corner round radius
        /// </summary>
        [Browsable(true), Category("AdvancedPanel"), Description("The corner round radius.")]
        public int RectRadius
        {
            get => _rectRadius;
            set
            {
                _rectRadius = value;
                Invalidate();
            }
        }

        #endregion

        public Block()
        {
            Size = new Size(50, 50);
            Paint += AdvancedPanel_Paint;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        #region Paint

        private void AdvancedPanel_Paint(object sender, PaintEventArgs e)
        {
            var panelRect = new Rectangle(0,
                                          0,
                                          Width - _shadowShift - 1,
                                          Height - _shadowShift - 1);

            DrawRectRaised(e.Graphics, panelRect);
        }

        private void DrawRectRaised(Graphics graphics, Rectangle rect)
        {
            edgeColor1 = ControlPaint.Light(_startColor);
            edgeColor2 = ControlPaint.Dark(_endColor);

            DrawEdges(graphics, ref rect);
            rect.Inflate(-_edgeWidth, -_edgeWidth);
            DrawPanelStyled(graphics, rect);
        }


        /// <summary>
        /// Fill in the panel edges
        /// </summary>
        /// <param name="g">Graphics Object</param>
        /// <param name="edgeRect">Rectangle defining the panel edge</param>
        protected virtual void DrawEdges(Graphics g, ref Rectangle edgeRect)
        {
            Rectangle lgbRect = edgeRect;
            lgbRect.Inflate(1, 1);

            // Blend colors 
            var edgeBlend = new Blend
            {
                Positions = new float[] { 0.0f, .45f, .51f, 1.0f },
                Factors = new float[] { .0f, .0f, .2f, 1f }
            };

            using (var edgeBrush = new LinearGradientBrush(lgbRect,
                                                edgeColor1,
                                                edgeColor2,
                                                LinearGradientMode.ForwardDiagonal))
            {
                edgeBrush.Blend = edgeBlend;
                RoundedRectangle.DrawFilledRoundedRectangle(g, edgeBrush, edgeRect, _rectRadius);
            }
        }

        /// <summary>
        /// Fill in the main panel with gradient
        /// </summary>
        /// <param name="g">Graphics Object</param>
        /// <param name="rect">Rectangle defining the panel top</param>
        protected virtual void DrawPanelStyled(Graphics g, Rectangle rect)
        {
            using (Brush pgb = new LinearGradientBrush(rect, _startColor, _endColor, LinearGradientMode.ForwardDiagonal))
            {
                RoundedRectangle.DrawFilledRoundedRectangle(g, pgb, rect, _rectRadius);
            }
        }
        #endregion
    }
}
