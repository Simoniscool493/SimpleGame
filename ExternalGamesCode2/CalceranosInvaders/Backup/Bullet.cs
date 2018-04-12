/*
 * Creato da SharpDevelop.
 * Utente: Giovanni Calcerano
 * Data: 24/05/2008
 * Ora: 19.16
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Windows.Forms;

namespace CalceranosInvaders
{
	/// <summary>
	/// Description of Bullet.
	/// </summary>
	public class Bullet
	{
		private int _x;
		
		private int _y;
		
		private bool _isActive;
		
		private Label _bulletLabel;
		private int _maxY;
		private int _minY;
		private int _addY;
		
		public Bullet(Label bulletLabel)
		{
			_bulletLabel=bulletLabel;
			_isActive=false;
			_bulletLabel.Visible=false;
		}
		
		public void Start(int x,int y,int maxY,int minY,int addY)
		{
			if (_isActive)
			{
				return;
			}
			_x=x;
			_y=y;
			_bulletLabel.Top=_y;
			_bulletLabel.Left=_x;
			_maxY=maxY;
			_minY=minY;
			_addY=addY;
			_isActive=true;
			_bulletLabel.Visible=true;
		}
		
		public void Stop()
		{
			_isActive=false;
			_bulletLabel.Visible=false;
			
		}
		
		public void Move()
		{
			if (!_isActive)
			{
				return;
			}
			_y+=_addY;
			if (_y>_maxY || _y<_minY)
			{
				_bulletLabel.Visible=false;
				_isActive=false;
				return;
			}
			_bulletLabel.Top=_y;
		}

		public bool IsActive {
			get { return _isActive; }
		}

		public int X {
			get { return _x; }
		}

		public int Y {
			get { return _y; }
		}
	}
}
