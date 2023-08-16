using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace BevelPanel
{
    public class Block : Panel
    {
        #region Variables

        /// <summary>Panel border style.</summary>
        public enum BevelStyle
        {
            /// <summary>Raised border.</summary>
            Raised,
        }

        public enum PanelGradientMode
        {
            /// <summary>Specifies a gradient from upper right to lower left.</summary>
            BackwardDiagonal = 3,

            /// <summary>Specifies a gradient from upper left to lower right.</summary>
            ForwardDiagonal = 2,

            /// <summary>Specifies a gradient from left to right.</summary>
            Horizontal = 0,

            /// <summary>Specifies a gradient from top to bottom.</summary>
            Vertical = 1
        }

        private Color _startColor = Color.DimGray;
        private Color _endColor = Color.DarkGray;
        private Color _borderColor = Color.FromArgb(102, 102, 102);
        private Color mainColor;

        private int _rectRadius = 5;
        private PanelGradientMode _backgroundGradientMode = PanelGradientMode.Vertical;
        private const int sh = 10;
        private int _edgeWidth = 2;

        private Color edgeColor1;
        private Color edgeColor2;
        private BevelStyle _style = BevelStyle.Raised;

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
        /// Gets or sets the style of the bevel
        /// </summary>
        [Browsable(true), Category("AdvancedPanel"), Description("The style of the bevel.")]
        public BevelStyle Style
        {
            get => _style;
            set
            {
                _style = value;
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
        /// Gets or sets the background gradient mode
        /// </summary>
        [Browsable(true), Category("AdvancedPanel"), Description("The gradient type.")]
        public PanelGradientMode BackgroundGradientMode
        {
            get => _backgroundGradientMode;
            set
            {
                _backgroundGradientMode = value;
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
            this.Size = new Size(50, 50);
            this.Paint += this.AdvancedPanel_Paint;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        #region Paint

        private void AdvancedPanel_Paint(object sender, PaintEventArgs e)
        {
            var panelRect = new Rectangle();

            // Draws the Panel
            switch (Style)
            {
                case BevelStyle.Raised:
                    DrawRectRaised(e.Graphics, panelRect);
                    break;
            }
        }

        private void DrawRectRaised(Graphics graphics, Rectangle rect)
        {
            var darknessEnd = _endColor.GetSaturation();
            var darknessBegin = _startColor.GetSaturation();
            mainColor = darknessEnd >= darknessBegin ? _endColor : _startColor;

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
            var edgeBlend = new Blend();
            if (RectRadius >= 150)
            {
                edgeBlend.Positions = new float[] { 0.0f, .2f, .4f, .6f, .8f, 1.0f };
                edgeBlend.Factors = new float[] { .0f, .0f, .2f, .4f, 1f, 1f };
            }
            else
            {
                switch (Style)
                {
                    case BevelStyle.Raised:
                        edgeBlend.Positions = new float[] { 0.0f, .45f, .51f, 1.0f };
                        edgeBlend.Factors = new float[] { .0f, .0f, .2f, 1f };
                        break;
                }
            }
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
            using (Brush pgb = new LinearGradientBrush(rect,
                                                       _startColor,
                                                       _endColor,
                                                       (LinearGradientMode)this.BackgroundGradientMode))
            {
                RoundedRectangle.DrawFilledRoundedRectangle(g, pgb, rect, _rectRadius);
            }
        }

        #endregion
    }
}
