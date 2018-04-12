/*
 * Creato da SharpDevelop.
 * Utente: Giovanni Calcerano
 * Data: 24/05/2008
 * Ora: 18.39
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Windows.Forms;

namespace CalceranosInvaders
{
	/// <summary>
	/// Description of Ship.
	/// </summary>
	public class Ship
	{
		private int _x;
		
		private int _y;
		
		private int _minX;
		private int _maxX;
		private int _addX;
		private Label _shipLabel;
		private bool _isActive;
		
		
		public Ship(Label shipLabel)
		{
			_shipLabel=shipLabel;
			this.Stop();
		}
		
		public void Start(int x,int y,int minX,int maxX,int addX)
		{
			_x=x;
			_y=y;
			_minX=minX;
			_maxX=maxX;
			_addX=addX;
			_shipLabel.Left=_x;
			_shipLabel.Top=_y;
			_shipLabel.Visible=true;
			_isActive=true;
		}
		
		public void Freeze()
		{
			_isActive=false;						
		}

		public void Defreeze()
		{
			_isActive=true;						
		}
		
		public void Stop()
		{
			_shipLabel.Visible=true;
			_isActive=false;			
		}
		public void MoveLeft()
		{
			if (!_isActive)
			{
				return;
			}
			_x-=_addX;
			if (_x<_minX)
			{
				_x=_minX;
			}
			_shipLabel.Left=_x;
		}
		
		public void MoveRight()
		{
			if (!_isActive)
			{
				return;
			}
			_x+=_addX;
			if (_x>_maxX)
			{
				_x=_maxX;
			}
			_shipLabel.Left=_x;
			
		}
		public int X {
			get { return _x; }
		}

		public int Y {
			get { return _y; }
		}

		public bool IsActive {
			get { return _isActive; }
		}
		
	}
}
