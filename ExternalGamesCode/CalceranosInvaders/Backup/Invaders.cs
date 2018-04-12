/*
 * Creato da SharpDevelop.
 * Utente: Giovanni Calcerano
 * Data: 25/05/2008
 * Ora: 8.54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Windows.Forms;


namespace CalceranosInvaders
{
	/// <summary>
	/// Description of Invaders.
	/// </summary>
	/// 
	public class Invaders
	{
		private int _x;
		private int _y;
		private Panel _invadersPanel;
		private Panel _invadersPanel1;
		private int _addX;
		private int _addY;
		private int _minX;
		private int _maxX;
		private int _minY;
		private int _maxY;
		private int _delay;

		private int _delayCounter;
		private int _leftColumn;
		
		private int _bottomRow;
		private int _rightColumn;
		
		private int _invadersToKill;
		private bool _complete;
		private int _invadersXDistance;
		private int _invadersYDistance;
		private bool _isActive;
		private int[] _invadersToKillForColumn=new int[11];
		private int[] _invadersToKillForRow=new int[5];
		private System.Collections.Generic.List<int> _columnsForShooting;
		private bool[,] _isInvaderAlive=new bool[5,11];
		
		private bool _changeDirectionAndGoDown;
		private System.Random _random;
		private int _invadersWidth;
		private int _invadersHeight;
		
		public Invaders(Panel invadersPanel,Panel invadersPanel1)
		{
			_invadersPanel=invadersPanel;
			_invadersPanel1=invadersPanel1;
			_invadersPanel1.SendToBack();
			_invadersPanel.SendToBack();
			_columnsForShooting=new System.Collections.Generic.List<int>();
			_random=new Random();
			this.Stop();
		}
		
		public void Start(int x,int y,int addX,int addY,int minX,int maxX,int minY,int maxY,int delay)
		{
			_x=x;
			_y=y;
			_addX=addX;
			_addY=addY;
			_minX=minX;
			_maxX=maxX;
			_minY=minY;
			_maxY=maxY;
			_delay=delay;
			_delayCounter=0;
			_invadersPanel.Left=_x;
			_invadersPanel.Top=_y;
			_leftColumn=0;
			_rightColumn=10;
			_invadersToKill=55;
			_complete=false;
			_invadersXDistance=_invadersPanel.Controls["lb_Invader01"].Left-_invadersPanel.Controls["lb_Invader00"].Left;
			_invadersYDistance=_invadersPanel.Controls["lb_Invader10"].Top-_invadersPanel.Controls["lb_Invader00"].Top;
			_invadersPanel1.Visible=false;
			_invadersPanel.Visible=false;
			this.Reset();
			_invadersPanel1.SendToBack();
			_invadersPanel.Visible=true;
			_invadersPanel1.Visible=false;
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
			_isActive=false;	
			_invadersPanel.Visible=false;
			_invadersPanel1.Visible=false;
		}
		
		public void Move()
		{
			if (!_isActive)
			{
				return;
			}

			
			if (_delayCounter<_delay)
			{
				_delayCounter+=1;
				return;
			}
			_delayCounter=0;
			if (_changeDirectionAndGoDown)
			{
				_addX=-_addX;
				_y+=_addY;				
				_changeDirectionAndGoDown=false;
			}
			else
			{
				_x+=_addX;
				if (_x<_minX)
				{
					_x=_minX;
					_changeDirectionAndGoDown=true;
				}
				else if (_x>_maxX)
				{
					_x=_maxX;
					_changeDirectionAndGoDown=true;
				}
			}
			
			_invadersPanel.Left=_x;
			_invadersPanel.Top=_y;		
			_invadersPanel1.Left=_x;
			_invadersPanel1.Top=_y;		
			if (_invadersPanel1.Visible)
			{
				_invadersPanel1.SendToBack();
				_invadersPanel.Visible=true;
				_invadersPanel1.Visible=false;
				
			}
			else
			{
				_invadersPanel.SendToBack();
				_invadersPanel1.Visible=true;
				_invadersPanel.Visible=false;
				
			}
		}

		public int BottomX0ForColumn(int column)
		{
			string columnString;
			for (int i=4;i>=0;i=i-1)
			{
				if (_isInvaderAlive[i,column])
				{

					if (column==10)
					{
						columnString="A";
					}
					else
					{
						columnString=column.ToString();
					}
					return this.X0+this._invadersPanel.Controls["lb_Invader"+i.ToString()+columnString].Left;					
				}
			}
			return -1;
		}

		public int BottomX1ForColumn(int column)
		{
			string columnString;
			for (int i=4;i>=0;i=i-1)
			{
				if (_isInvaderAlive[i,column])
				{

					if (column==10)
					{
						columnString="A";
					}
					else
					{
						columnString=column.ToString();
					}
					return this.X0+this._invadersPanel.Controls["lb_Invader"+i.ToString()+columnString].Left+this._invadersPanel.Controls["lb_Invader"+i.ToString()+columnString].Width;					
				}
			}
			return -1;
		}

		public int BottomY0ForColumn(int column)
		{
			string columnString;
			for (int i=4;i>=0;i=i-1)
			{
				if (_isInvaderAlive[i,column])
				{

					if (column==10)
					{
						columnString="A";
					}
					else
					{
						columnString=column.ToString();
					}
					return this.Y0+this._invadersPanel.Controls["lb_Invader"+i.ToString()+columnString].Top;					
				}
			}
			
			return -1;
		}
		
		public int BottomY1ForColumn(int column)
		{
			string columnString;
			for (int i=4;i>=0;i=i-1)
			{
				if (_isInvaderAlive[i,column])
				{

					if (column==10)
					{
						columnString="A";
					}
					else
					{
						columnString=column.ToString();
					}
					return this.Y0+this._invadersPanel.Controls["lb_Invader"+i.ToString()+columnString].Top+this._invadersPanel.Controls["lb_Invader"+i.ToString()+columnString].Height;					
				}
			}
			return -1;
		}
		
		
		public void StartInvadersBullet(Bullet bullet)
		{
			if (!_isActive)
			{
				return;
			}
			int columnIndexer=_random.Next(0,_columnsForShooting.Count-1);
			int column=_columnsForShooting[columnIndexer];
			string columnString;
			for (int i=4;i>=0;i=i-1)
			{
				if (_isInvaderAlive[i,column])
				{

					if (column==10)
					{
						columnString="A";
					}
					else
					{
						columnString=column.ToString();
					}
					
					bullet.Start(this.X0+this._invadersPanel.Controls["lb_Invader"+i.ToString()+columnString].Left,this.Y0+this._invadersPanel.Controls["lb_Invader"+i.ToString()+columnString].Top+this._invadersPanel.Controls["lb_Invader"+i.ToString()+columnString].Height,this._invadersPanel.Parent.Height,0,6);
					break;
				}
			}
			
		}
		
		public int X0 {
			get { return _x; }
		}

		public int X1 {
			get { return _x+_invadersWidth; }
		}

		public int Y0 {
			get { return _y; }
		}

		public int Y1 {
			get { return _y+_invadersHeight; }
		}

		public int LeftColumn {
			get { return _leftColumn; }
		}
		
		public int RightColumn {
			get { return _rightColumn; }
		}
		
		public bool IsComplete {
			get { return _complete; }
		}
		
		public int Delay {
			get { return _delay; }
			set 
			{ 
				if (value<0)
				{
					_delay = 0;				
				}
				else
				{
					_delay = value;
				}
			}
		}
		
		public int InvadersToKill {
			get { return _invadersToKill; }
		}
		
		private void CheckBottomRow()
		{
			bool isRowCompleted;
			int rowsCompleted=0;
			do
			{
				isRowCompleted=(_invadersToKillForRow[this._bottomRow]==0);
				if (isRowCompleted)
				{
					rowsCompleted+=1;
					_bottomRow-=1;
				}
				else
				{
					break;
				}
			} while(true);
			if (rowsCompleted>0)
			{
					_invadersHeight-=_invadersYDistance*rowsCompleted;				
			}
		}
		private void CheckLeftOrRightColumn(bool checkingForLeft)
		{
			bool isColumnCompleted;
			int column;
			int addColumn;
			int columnsCompleted=0;
			
			if (checkingForLeft)
			{
				column=_leftColumn;
				addColumn=1;
			}
			else
			{
				column=_rightColumn;
				addColumn=-1;
			}
			do
			{
				isColumnCompleted=(_invadersToKillForColumn[column]==0);
				if (isColumnCompleted)
				{
					columnsCompleted+=1;
					column+=addColumn;
				}
				else
				{
					break;
				}
			} while(true);
			
			if (columnsCompleted>0)
			{
				if (checkingForLeft)
				{
					_leftColumn=column;
					_minX-=_invadersXDistance*columnsCompleted;
				}
				else
				{
					_rightColumn=column;
					_maxX+=_invadersXDistance*columnsCompleted;
					_invadersWidth-=_invadersXDistance*columnsCompleted;
				}

			}
		}

		
		private void Reset()
		{
			string row;
			string column;
			for (int i=0;i<=4;i=i+1)
			{
				row=i.ToString();
				for (int j=_leftColumn;j<=_rightColumn;j=j+1)
				{
					if (j==10)
					{
						column="A";
					}
					else
					{
						column=j.ToString();
					}
					_invadersPanel.Controls["lb_Invader"+row+column].Visible=true;
					_invadersPanel1.Controls["label"+((5-i)+(10-j)*5).ToString()].Visible=true;
					_isInvaderAlive[i,j]=true;
				}
			}
			for (int i=0;i<=4;i=i+1)
			{
				_invadersToKillForRow[i]=11;
			}
			for (int j=_leftColumn;j<=_rightColumn;j=j+1)
			{
				_invadersToKillForColumn[j]=5;
				_columnsForShooting.Add(j);
			}
			_invadersWidth=this._invadersPanel.Width;
			_bottomRow=4;
			_invadersHeight=this._invadersPanel.Height;
		}
		
		public int CheckHit(int x,int y)
		{
			string row;
			string column;
			int i=(y-_y)/22;
			int j=(x-_x)/34;
			if (i<0 || i>4 || j<_leftColumn || j>_rightColumn)
			{
				return 0;
			}
			row=i.ToString();
			if (j==10)
			{
				column="A";
			}
			else
			{
				column=j.ToString();
			}
			if (_isInvaderAlive[i,j])
			{
				if (x>=_invadersPanel.Controls["lb_Invader"+row+column].Left+_x && x<=_invadersPanel.Controls["lb_Invader"+row+column].Left+_x+_invadersPanel.Controls["lb_Invader"+row+column].Width && y>=_invadersPanel.Controls["lb_Invader"+row+column].Top+_y && y<=_invadersPanel.Controls["lb_Invader"+row+column].Top+_y+_invadersPanel.Controls["lb_Invader"+row+column].Height)
				{
					_invadersPanel.Controls["lb_Invader"+row+column].Visible=false;
					_invadersPanel1.Controls["label"+((5-i)+(10-j)*5).ToString()].Visible=false;
					_isInvaderAlive[i,j]=false;
					_invadersToKillForColumn[j]-=1;
					_invadersToKillForRow[i]-=1;
					
					_invadersToKill-=1;
					int points=0;
					if (i==4 || i==3)
					{
						points=10;
					}
					if (i==2 || i==1)
					{
						points=20;
					}
					if (i==0)
					{
						points=40;
					}
					if (_invadersToKill==0)
					{
						_complete=true;
						return points;
					}
					if (_invadersToKillForColumn[j]==0)
					{
						this._columnsForShooting.Remove(j);
					}
					if (j==_leftColumn)
					{
						CheckLeftOrRightColumn(true);
					}
					else if (j==_rightColumn)
					{
						CheckLeftOrRightColumn(false);
					}
					if (i==_bottomRow)
					{
						CheckBottomRow();
					}
					return points;
				}
				
			}
			return 0;
		}
			
	}
}
