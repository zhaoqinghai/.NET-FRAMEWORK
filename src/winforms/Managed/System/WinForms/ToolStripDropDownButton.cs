//------------------------------------------------------------------------------
// <copyright file="ToolStripDropDownButton.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>                                                                
//------------------------------------------------------------------------------

namespace System.Windows.Forms {
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using Microsoft.Win32;
    using System.Drawing.Imaging;
    using System.Threading;
    using System.Diagnostics;
    using System.Windows.Forms.ButtonInternal;
    using System.Windows.Forms.Layout;
    using System.Security.Permissions;
    using System.Security;
    using System.Windows.Forms.Design; 
    /// <include file='doc\ToolStripPopupButton.uex' path='docs/doc[@for="ToolStripDropDownButton"]/*' />
    /// <devdoc>
    /// A ToolStripButton that can display a popup.
    /// </devdoc>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripDropDownButton : ToolStripDropDownItem {

        private bool showDropDownArrow = true;
        private byte openMouseId = 0;
            
        /// <include file='doc\ToolStripPopupButton.uex' path='docs/doc[@for="ToolStripDropDownButton.ToolStripDropDownButton"]/*' />
        /// <devdoc>
        /// Constructs a ToolStripButton that can display a popup.
        /// </devdoc>
        public ToolStripDropDownButton() {
            Initialize();
        }
        public ToolStripDropDownButton(string text):base(text,null,(EventHandler)null) {
            Initialize();            
        }
        public ToolStripDropDownButton(Image image):base(null,image,(EventHandler)null) {
            Initialize();            
        }
        public ToolStripDropDownButton(string text, Image image):base(text,image,(EventHandler)null) {
            Initialize();            
        }
        public ToolStripDropDownButton(string text, Image image, EventHandler onClick):base(text,image,onClick) {
            Initialize();            
        }
        public ToolStripDropDownButton(string text, Image image, EventHandler onClick, string name) :base(text,image,onClick,name){
            Initialize(); 
        }
        public ToolStripDropDownButton(string text, Image image, params ToolStripItem[] dropDownItems):base(text,image,dropDownItems) {
            Initialize();            
        }
 
        [DefaultValue(true)]
        public new bool AutoToolTip {
            get { 
                return base.AutoToolTip;
            }
            set {
                base.AutoToolTip = value;
            }
        }

        
        protected override bool DefaultAutoToolTip {
            get { 
               return true; 
            }
        }

        /// <include file='doc\ToolStripPopupButton.uex' path='docs/doc[@for="ToolStripDropDownButton.ShowDropDownArrow"]/*' />
        [
        DefaultValue(true),
        SRDescription(SR.ToolStripDropDownButtonShowDropDownArrowDescr),
        SRCategory(SR.CatAppearance)
        ]
        public bool ShowDropDownArrow {
            get {
                return showDropDownArrow;
            }
            set {
                if (showDropDownArrow != value) {
                    showDropDownArrow = value;
                    this.InvalidateItemLayout(PropertyNames.ShowDropDownArrow);      
                }
            }
        }
        /// <devdoc>
        /// Creates an instance of the object that defines how image and text
        /// gets laid out in the ToolStripItem
        /// </devdoc>
        internal override ToolStripItemInternalLayout CreateInternalLayout() {
            return new ToolStripDropDownButtonInternalLayout(this);
        }

        
        protected override ToolStripDropDown CreateDefaultDropDown() {
             // AutoGenerate a Winbar DropDown - set the property so we hook events
              return new ToolStripDropDownMenu(this, /*isAutoGenerated=*/true);
        }

        /// <devdoc>
        /// Called by all constructors of ToolStripButton.
        /// </devdoc>
        private void Initialize() {
            SupportsSpaceKey = true;            
        }
                
        /// <include file='doc\ToolStripPopupButton.uex' path='docs/doc[@for="ToolStripDropDownButton.OnMouseDown"]/*' />
        /// <devdoc>
        /// Overriden to invoke displaying the popup.
        /// </devdoc>
        protected override void OnMouseDown(MouseEventArgs e) {
            if ((Control.ModifierKeys != Keys.Alt) &&
                (e.Button == MouseButtons.Left)) {
                    if (DropDown.Visible) {                        
                        ToolStripManager.ModalMenuFilter.CloseActiveDropDown(DropDown, ToolStripDropDownCloseReason.AppClicked);
                    }
                    else {                        
                        // opening should happen on mouse down.  
                        Debug.Assert(ParentInternal != null, "Parent is null here, not going to get accurate ID");
                        openMouseId = (ParentInternal == null) ? (byte)0: ParentInternal.GetMouseId();
                        this.ShowDropDown(/*mousePush =*/true);
                    }
            }
            base.OnMouseDown(e);
        } 

        protected override void OnMouseUp(MouseEventArgs e) {
            if ((Control.ModifierKeys != Keys.Alt) &&
                (e.Button == MouseButtons.Left)) {
                Debug.Assert(ParentInternal != null, "Parent is null here, not going to get accurate ID");
                byte closeMouseId = (ParentInternal == null) ? (byte)0: ParentInternal.GetMouseId();
                if (closeMouseId != openMouseId) {
                    openMouseId = 0;  // reset the mouse id, we should never get this value from toolstrip.
                    ToolStripManager.ModalMenuFilter.CloseActiveDropDown(DropDown, ToolStripDropDownCloseReason.AppClicked);
                    Select();
                }
            }
            base.OnMouseUp(e);            
        }

        protected override void OnMouseLeave(EventArgs e) {
            openMouseId = 0;  // reset the mouse id, we should never get this value from toolstrip.
            base.OnMouseLeave(e);                    
        }
        /// <include file='doc\ToolStripPopupButton.uex' path='docs/doc[@for="ToolStripDropDownButton.OnPaint"]/*' />
        /// <devdoc>
        /// Inheriting classes should override this method to handle this event.
        /// </devdoc>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {        
                
            if (this.Owner != null) {
                ToolStripRenderer renderer = this.Renderer;
                Graphics g = e.Graphics;
                
                renderer.DrawDropDownButtonBackground(new ToolStripItemRenderEventArgs(e.Graphics, this));
        
                if ((DisplayStyle & ToolStripItemDisplayStyle.Image) == ToolStripItemDisplayStyle.Image) { 
                    renderer.DrawItemImage(new ToolStripItemImageRenderEventArgs(g, this, InternalLayout.ImageRectangle));
                }

                if ((DisplayStyle & ToolStripItemDisplayStyle.Text) == ToolStripItemDisplayStyle.Text) { 
                     renderer.DrawItemText(new ToolStripItemTextRenderEventArgs(g, this, this.Text, InternalLayout.TextRectangle, this.ForeColor, this.Font, InternalLayout.TextFormat));
                }
                if (ShowDropDownArrow) {
                    ToolStripDropDownButton.ToolStripDropDownButtonInternalLayout layout = InternalLayout as ToolStripDropDownButtonInternalLayout;
                    Rectangle dropDownArrowRect = (layout != null) ? layout.DropDownArrowRect :Rectangle.Empty;

                    Color arrowColor =  Enabled ? SystemColors.ControlText : SystemColors.ControlDark;
                    renderer.DrawArrow(new ToolStripArrowRenderEventArgs(g, this,dropDownArrowRect, arrowColor, ArrowDirection.Down)); 
                }
            }
        }


        /// <include file='doc\ToolStripPopupButton.uex' path='docs/doc[@for="ToolStripDropDownButton.ProcessMnemonic"]/*' />

        [UIPermission(SecurityAction.LinkDemand, Window=UIPermissionWindow.AllWindows)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")] // 'charCode' matches control.cs
        protected internal override bool ProcessMnemonic(char charCode) {
             // checking IsMnemonic is not necesssary - toolstrip does this for us.
             if (HasDropDownItems) {
                 Select();
                 ShowDropDown();
                 return true;
             }
             return false;
         }


        internal class ToolStripDropDownButtonInternalLayout : ToolStripItemInternalLayout {
            private ToolStripDropDownButton    ownerItem;
            private static readonly Size       dropDownArrowSizeUnscaled = new Size(5, 3);
            private static Size                dropDownArrowSize = dropDownArrowSizeUnscaled;
            private const int                  DROP_DOWN_ARROW_PADDING = 2;
            private static Padding             dropDownArrowPadding = new Padding(DROP_DOWN_ARROW_PADDING);
            private Padding                    scaledDropDownArrowPadding = dropDownArrowPadding;
            private Rectangle                  dropDownArrowRect    = Rectangle.Empty;
            
            public ToolStripDropDownButtonInternalLayout(ToolStripDropDownButton ownerItem) : base(ownerItem) {
                if (DpiHelper.IsScalingRequired) {
                    // these 2 values are used to calculate size of the clickable drop down button
                    // on the right of the image/text
                    dropDownArrowSize = DpiHelper.LogicalToDeviceUnits(dropDownArrowSizeUnscaled);
                    scaledDropDownArrowPadding = DpiHelper.LogicalToDeviceUnits(dropDownArrowPadding);
                }
                this.ownerItem = ownerItem;    
            }

            public override Size GetPreferredSize(Size constrainingSize) {

                Size preferredSize = base.GetPreferredSize(constrainingSize);
                if (ownerItem.ShowDropDownArrow) {
                    if (ownerItem.TextDirection == ToolStripTextDirection.Horizontal) {
                        preferredSize.Width += DropDownArrowRect.Width + scaledDropDownArrowPadding.Horizontal;
                    }
                    else {
                        preferredSize.Height += DropDownArrowRect.Height + scaledDropDownArrowPadding.Vertical;
                    }
                }
                return preferredSize;
            }

            protected override ToolStripItemLayoutOptions CommonLayoutOptions() {
               ToolStripItemLayoutOptions options = base.CommonLayoutOptions();

               if (ownerItem.ShowDropDownArrow) {

                    if (ownerItem.TextDirection == ToolStripTextDirection.Horizontal) {

                        // We're rendering horizontal....  make sure to take care of RTL issues.
                        
                        int widthOfDropDown = dropDownArrowSize.Width + scaledDropDownArrowPadding.Horizontal;
                        options.client.Width -= widthOfDropDown;

                        if (ownerItem.RightToLeft == RightToLeft.Yes) {

                            // if RightToLeft.Yes: [ v | rest of drop down button ]
                            options.client.Offset(widthOfDropDown, 0);
                            dropDownArrowRect = new Rectangle(scaledDropDownArrowPadding.Left,0, dropDownArrowSize.Width, ownerItem.Bounds.Height);
                        }
                        else {
                           // if RightToLeft.No [ rest of drop down button | v ]
                           dropDownArrowRect = new Rectangle(options.client.Right,0, dropDownArrowSize.Width, ownerItem.Bounds.Height);
                            
                        }
                    }
                    else {
                        // else we're rendering vertically. 
                        int heightOfDropDown = dropDownArrowSize.Height + scaledDropDownArrowPadding.Vertical;

                        options.client.Height -= heightOfDropDown;
                        
                        //  [ rest of button / v]
                        dropDownArrowRect = new Rectangle(0,options.client.Bottom + scaledDropDownArrowPadding.Top, ownerItem.Bounds.Width-1, dropDownArrowSize.Height);
                       
                    }

               }
               return options;
            }

            public Rectangle DropDownArrowRect {
                get {
                      return dropDownArrowRect;
                }
            }
            
        }

    }
}
    

