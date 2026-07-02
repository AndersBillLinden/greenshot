/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2016 Thomas Braun, Jens Klingen, Robin Krom
 * 
 * For more information see: http://getgreenshot.org/
 * The Greenshot project is hosted on GitHub https://github.com/greenshot/greenshot
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace GreenshotPlugin.Controls {
	/// <summary>
	/// See: http://nickstips.wordpress.com/2010/03/03/c-panel-resets-scroll-position-after-focus-is-lost-and-regained/
	/// </summary>
	public class NonJumpingPanel : Panel {
		/// <summary>
		/// Raised when the mouse wheel is used without a modifier key. When a handler is attached the
		/// panel does NOT auto-scroll on a plain wheel, so the consumer can use it for e.g. zooming.
		/// </summary>
		public event EventHandler<MouseEventArgs> PlainMouseWheel;

		protected override Point ScrollToControl(Control activeControl) {
			// Returning the current location prevents the panel from
			// scrolling to the active control when the panel loses and regains focus
			return DisplayRectangle.Location;
		}

		/// <summary>
		/// A plain wheel is offered to PlainMouseWheel (and suppresses scrolling when handled), while
		/// shift + wheel scrolls horizontally.
		/// </summary>
		/// <param name="e">MouseEventArgs</param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			bool shift = (ModifierKeys & Keys.Shift) == Keys.Shift;
			if (!shift && PlainMouseWheel != null)
			{
				// Let the consumer (e.g. the editor) use the wheel; do not auto-scroll.
				PlainMouseWheel(this, e);
				return;
			}
			if (VScroll && shift)
			{
				VScroll = false;
				base.OnMouseWheel(e);
				VScroll = true;
			}
			else
			{
				base.OnMouseWheel(e);
			}
		}
	}
}
