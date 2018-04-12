/*
 * Creato da SharpDevelop.
 * Utente: Giovanni Calcerano
 * Data: 24/05/2008
 * Ora: 17.35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace CalceranosInvaders
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
	
		[DllImport("user32")]
	
		static extern short GetAsyncKeyState(Keys vKey);

		private Ship _ship;
		private Bullet _shipBullet;
		private Bullet _invadersBullet;
		private Bullet _invadersBullet1;
		private Invaders _invaders;
		private int _shipXStart;
		private int _shipYStart;
		private int _invadersXStart;
		private int _invadersYStart;
		private int _nextMilestoneToIncreaseSpeed;
		private System.Collections.Generic.Stack<int> _milestonesToIncreaseSpeed;
		private int _points;
		private System.Random _random;
		private double _valueForInvadersBullet;
		private double _valueForInvadersBullet1;
		private bool _shipExploded;
		private int _delayExploded;
		private int _delaySequence;
		private int _lives;
		private int _pointsToGetNewLive;
		private int _invadersDelayValue;
		private bool[,,] _bunkersGrid;
		private Graphics[] _objGraphics;
		private Pen _pen;
		private Bitmap[] _bunkersBitmap;
		private bool _stillInside=false;
		private System.Drawing.Image _bunkerImageBackup;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			_random = new Random();
		}
		
		void Tm_GetKeysTick(object sender, EventArgs e)
		{
			if (this._shipExploded)
			{
				if (_delayExploded==0 && _delaySequence==0)
				{
					if ( _lives>0)
					{
						_shipExploded=false;
						this.lb_Ship.Visible=true;
						this.lb_ShipExploded1.Visible=false;
						_ship.Defreeze();
						_invaders.Defreeze();
						return;
					}
					else
					{
						tm_GetKeys.Enabled=false;
						this.lb_GameOver.Visible=true;
					}
					
				}
				else
				{
					_delayExploded-=1;
					if (_delayExploded==0)
					{
						if (_delaySequence==3)
						{
							this.lb_ShipExploded1.Left=this.lb_Ship.Left;
							this.lb_ShipExploded1.Top=this.lb_Ship.Top-(this.lb_ShipExploded.Height-this.lb_Ship.Height);
							this.lb_ShipExploded1.Visible=true;
							this.lb_ShipExploded.Visible=false;
							_delayExploded=4;
							_delaySequence-=1;
						}
						else if (_delaySequence==2)
						{
							this.lb_ShipExploded2.Left=this.lb_Ship.Left;
							this.lb_ShipExploded2.Top=this.lb_Ship.Top-(this.lb_ShipExploded.Height-this.lb_Ship.Height);
							this.lb_ShipExploded2.Visible=true;
							this.lb_ShipExploded1.Visible=false;
							_delayExploded=4;
							_delaySequence-=1;
							
						}
						else if (_delaySequence==1)
						{
							this.lb_ShipExploded2.Visible=false;
							_delayExploded=20;
							_delaySequence-=1;
							this.lb_Ship.Visible=false;
							
						}
					}
				}
				return;
			}
			if ( GetAsyncKeyState(Keys.Left) < 0)
			{
				_ship.MoveLeft();
				
			}
			if (GetAsyncKeyState(Keys.Right) < 0)
			{
				_ship.MoveRight();
			}

			if ((!_shipBullet.IsActive) && GetAsyncKeyState(Keys.Space) < 0)
			{
				if (_ship.IsActive)
				{
					_shipBullet.Start(_ship.X+15,_ship.Y-lb_Bullet.Height,this.Height,0,-6);
				}
			}
			
		}
		
		void MainFormLoad(object sender, EventArgs e)
		{
			_ship=new Ship(lb_Ship);
			this._shipXStart=lb_Ship.Left;
			this._shipYStart=lb_Ship.Top;
			_ship.Start(_shipXStart,_shipYStart,0,this.Width-lb_Ship.Width,6);
			_shipBullet=new Bullet(this.lb_Bullet);
			_invadersBullet=new Bullet(this.lb_InvadersBullet);
			_invadersBullet1=new Bullet(this.lb_InvadersBullet1);
			_invaders=new Invaders(this.pn_InvadersContainer,this.panel1);
			this._invadersXStart=pn_InvadersContainer.Left;
			this._invadersYStart=pn_InvadersContainer.Top;
			_invadersDelayValue=12;
			_invaders.Start(_invadersXStart,_invadersYStart,-5,22,10,this.Width-pn_InvadersContainer.Width-40,0,this.Height-pn_InvadersContainer.Height,_invadersDelayValue);
			_invadersYStart+=22;
			_milestonesToIncreaseSpeed=new System.Collections.Generic.Stack<int>();
			RestartMilestonesForSpeed();
			lb_DelayValue.Text=_invaders.Delay.ToString();
			_points=0;
			lb_PointsValue.Text=_points.ToString();
			_valueForInvadersBullet=0.98;
			_valueForInvadersBullet1=0.995;
			_shipExploded=false;
			_lives=3;
			_pointsToGetNewLive=2000;
            _pen = new Pen(Color.Black);
            RestartBunkers();

            this.pb_Bunker1.SendToBack();
            this.pb_Bunker2.SendToBack();
            this.pb_Bunker3.SendToBack();
            this.pb_Bunker4.SendToBack();
            _pen.Width=6;
            _bunkerImageBackup=pb_Bunker1.Image;//.Clone();

		}

		private void RestartBunkers()
		{
            Bitmap bmp = new Bitmap( pb_Bunker1.Image );
            if (_bunkersGrid==null)
            {
            	_bunkersGrid=new bool[4,bmp.Width,bmp.Height];
            }
            for (int XX=0;XX<bmp.Width;XX=XX+1)
            {

	            for (int YY=0;YY<bmp.Height;YY=YY+1)
	            {
		            Color clr=bmp.GetPixel(XX, YY);
		            if (clr.R==0 && clr.G==0 && clr.B==0)
		            {
		            	_bunkersGrid[0,XX,YY]=false;
		            	_bunkersGrid[1,XX,YY]=false;
		            	_bunkersGrid[2,XX,YY]=false;
		            	_bunkersGrid[3,XX,YY]=false;
		            }
		            else
		            {
		            	_bunkersGrid[0,XX,YY]=true;
		            	_bunkersGrid[1,XX,YY]=true;
		            	_bunkersGrid[2,XX,YY]=true;
		            	_bunkersGrid[3,XX,YY]=true;
		            }
	            	
	            }
            	
            }
            if (_bunkersBitmap==null)
            {
                        this._bunkersBitmap=new Bitmap[4];
            	
            }

            this._bunkersBitmap[0] = new Bitmap( pb_Bunker1.Image );
            this._bunkersBitmap[1] = new Bitmap( pb_Bunker2.Image );
            this._bunkersBitmap[2] = new Bitmap( pb_Bunker3.Image );
            this._bunkersBitmap[3] = new Bitmap( pb_Bunker4.Image );
            this._objGraphics=new Graphics[4];
            _objGraphics[0]= Graphics.FromImage(this._bunkersBitmap[0]);
            _objGraphics[1]= Graphics.FromImage(this._bunkersBitmap[1]);
            _objGraphics[2]= Graphics.FromImage(this._bunkersBitmap[2]);
            _objGraphics[3]= Graphics.FromImage(this._bunkersBitmap[3]);
			
		}
		
		private void RestartMilestonesForSpeed()
		{
			_milestonesToIncreaseSpeed.Clear();
			if (_invadersDelayValue==12)
			{
				_milestonesToIncreaseSpeed.Push(1);
				_milestonesToIncreaseSpeed.Push(2);
				_milestonesToIncreaseSpeed.Push(3);
				_milestonesToIncreaseSpeed.Push(5);
				_milestonesToIncreaseSpeed.Push(10);
				_milestonesToIncreaseSpeed.Push(15);
				_milestonesToIncreaseSpeed.Push(20);
				_milestonesToIncreaseSpeed.Push(25);
				_milestonesToIncreaseSpeed.Push(30);
				_milestonesToIncreaseSpeed.Push(35);
				_milestonesToIncreaseSpeed.Push(40);
				_milestonesToIncreaseSpeed.Push(45);
			}
			else if (_invadersDelayValue==10)
			{
				_milestonesToIncreaseSpeed.Push(1);
				_milestonesToIncreaseSpeed.Push(2);
				_milestonesToIncreaseSpeed.Push(3);
				_milestonesToIncreaseSpeed.Push(5);
				_milestonesToIncreaseSpeed.Push(10);
				_milestonesToIncreaseSpeed.Push(15);
				_milestonesToIncreaseSpeed.Push(20);
				_milestonesToIncreaseSpeed.Push(25);
				_milestonesToIncreaseSpeed.Push(30);
				_milestonesToIncreaseSpeed.Push(40);
			}
			else if (_invadersDelayValue==8)
			{
				_milestonesToIncreaseSpeed.Push(1);
				_milestonesToIncreaseSpeed.Push(2);
				_milestonesToIncreaseSpeed.Push(3);
				_milestonesToIncreaseSpeed.Push(5);
				_milestonesToIncreaseSpeed.Push(10);
				_milestonesToIncreaseSpeed.Push(20);
				_milestonesToIncreaseSpeed.Push(30);
				_milestonesToIncreaseSpeed.Push(40);
			}
			else if (_invadersDelayValue==6)
			{
				_milestonesToIncreaseSpeed.Push(1);
				_milestonesToIncreaseSpeed.Push(2);
				_milestonesToIncreaseSpeed.Push(5);
				_milestonesToIncreaseSpeed.Push(10);
				_milestonesToIncreaseSpeed.Push(25);
				_milestonesToIncreaseSpeed.Push(35);
			}

			_nextMilestoneToIncreaseSpeed=_milestonesToIncreaseSpeed.Pop();
		}

		private void CheckForBunker(Bullet _shipBullet,int x,int y,int bunkerCounter)
		{
            bool bunkerFound=false;
            for (int i=-2;i<=2;i=i+1)
            {
            	for (int j=-2;j<=2;j=j+1)
            	{
		            try
		            {
		            	bunkerFound=_bunkersGrid[bunkerCounter,x+i,y+j];
		            }
		            catch
		            {
		            	
		            }
		            if (bunkerFound)
		            {
		            	break;
		            }            		
            	}
	            if (bunkerFound)
	            {
	            	break;
	            }
            }
            if (bunkerFound)
            {
	           _shipBullet.Stop();		            	
	           _objGraphics[bunkerCounter].DrawLine(_pen, x, y+4, x, y-4);
                for (int i=-3;i<=3;i=i+1)
                {
	                for (int j=-4;j<=4;j=j+1)
	                {
	                	try
	                	{
		                	_bunkersGrid[bunkerCounter,x+i,y+j]=false;
	                		
	                	}
	                	catch
	                	{
	                		
	                	}
	                }
                	
                }
				if (bunkerCounter==0)
				{
	                this.pb_Bunker1.Image=this._bunkersBitmap[bunkerCounter];
				}
				else if (bunkerCounter==1)
				{
	                this.pb_Bunker2.Image=this._bunkersBitmap[bunkerCounter];
				}
				else if (bunkerCounter==2)
				{
	                this.pb_Bunker3.Image=this._bunkersBitmap[bunkerCounter];
				}
				else 
				{
	                this.pb_Bunker4.Image=this._bunkersBitmap[bunkerCounter];
				}
            }
			
		}
			
		private void PutRectangleOnBunker(int x0,int y0,int x1,int y1,int bunkerCounter)
		{
			SolidBrush solidBrush = new SolidBrush(Color.Black);
			_objGraphics[bunkerCounter].FillRectangle(solidBrush, x0, y0, x1, y1);

            for (int i=x0;i<=x1;i=i+1)
            {
                for (int j=y0;j<=y1;j=j+1)
                {
                	try
                	{
	                	_bunkersGrid[bunkerCounter,i,j]=false;
                		
                	}
                	catch
                	{
                		
                	}
                }
            	
            }
			if (bunkerCounter==0)
			{
                this.pb_Bunker1.Image=this._bunkersBitmap[bunkerCounter];
			}
			else if (bunkerCounter==1)
			{
                this.pb_Bunker2.Image=this._bunkersBitmap[bunkerCounter];
			}
			else if (bunkerCounter==2)
			{
                this.pb_Bunker3.Image=this._bunkersBitmap[bunkerCounter];
			}
			else 
			{
                this.pb_Bunker4.Image=this._bunkersBitmap[bunkerCounter];
			}
			
		}
		
		void Tm_MovementsTick(object sender, EventArgs e)
		{
			if (_stillInside)
			{
				return;
			}
			_stillInside=true;
			_invaders.Move();
			
			if (_invaders.Y1>=_ship.Y)
			{
				_delayExploded=4;
				_delaySequence=3;
				_shipExploded=true;
				_lives=0;
				_invadersBullet.Stop();
				_ship.Freeze();
				_invaders.Freeze();
				this.lb_ShipExploded.Left=this.lb_Ship.Left;
				this.lb_ShipExploded.Top=this.lb_Ship.Top-(this.lb_ShipExploded.Height-this.lb_Ship.Height);
				this.lb_ShipExploded.Visible=true;
				_ship.Stop();
				this.lb_ShipsValue.Text=_lives.ToString();
				lb_GameOver.Text="!!! INVASION !!!!";
				_stillInside=false;
				return;

			}
			bool _startedInvadersBullet=false;

			/*			
			if (_invaders.Y1>this.pb_Bunker1.Top)
			{
				for (int i=_invaders.LeftColumn; i<=_invaders.RightColumn;i=i+1)
				{
					
					int x0=_invaders.BottomX0ForColumn(i);
					if (x0==-1)
					{
						break;
					}
					int x1=_invaders.BottomX1ForColumn(i);
					int y0=_invaders.BottomY0ForColumn(i);
					int y1=_invaders.BottomY1ForColumn(i);
					bool leftTopCheck=_invaders.BottomX0ForColumn(i)>=this.pb_Bunker1.Left && _invaders.BottomX0ForColumn(i)<=this.pb_Bunker1.Left+this.pb_Bunker1.Width && _invaders.BottomY0ForColumn(i)>=this.pb_Bunker1.Top && _invaders.BottomX0ForColumn(i)<=this.pb_Bunker1.Top+this.pb_Bunker1.Height;
					bool leftBottomCheck=_invaders.BottomX0ForColumn(i)>=this.pb_Bunker1.Left && _invaders.BottomX0ForColumn(i)<=this.pb_Bunker1.Left+this.pb_Bunker1.Width && _invaders.BottomY1ForColumn(i)>=this.pb_Bunker1.Top && _invaders.BottomX1ForColumn(i)<=this.pb_Bunker1.Top+this.pb_Bunker1.Height;
					bool rightTopCheck=_invaders.BottomX1ForColumn(i)>=this.pb_Bunker1.Left && _invaders.BottomX1ForColumn(i)<=this.pb_Bunker1.Left+this.pb_Bunker1.Width && _invaders.BottomY0ForColumn(i)>=this.pb_Bunker1.Top && _invaders.BottomX0ForColumn(i)<=this.pb_Bunker1.Top+this.pb_Bunker1.Height;
					bool rightBottomCheck=_invaders.BottomX1ForColumn(i)>=this.pb_Bunker1.Left && _invaders.BottomX1ForColumn(i)<=this.pb_Bunker1.Left+this.pb_Bunker1.Width && _invaders.BottomY1ForColumn(i)>=this.pb_Bunker1.Top && _invaders.BottomX1ForColumn(i)<=this.pb_Bunker1.Top+this.pb_Bunker1.Height;
					if (leftTopCheck || leftBottomCheck || rightTopCheck || rightBottomCheck)
					{
						this.PutRectangleOnBunker(x0-this.pb_Bunker1.Left,y0-this.pb_Bunker1.Top,x1-this.pb_Bunker1.Left,y1-this.pb_Bunker1.Top,0);
					}
				}
			}
			*/
			if (_invadersBullet.IsActive)
			{
				_invadersBullet.Move();

				if (_invadersBullet.X>=this.pb_Bunker1.Left && _invadersBullet.X<=this.pb_Bunker1.Left+this.pb_Bunker1.Width && _invadersBullet.Y>=this.pb_Bunker1.Top && _invadersBullet.Y<=this.pb_Bunker1.Top+this.pb_Bunker1.Height)
				{
					CheckForBunker(_invadersBullet,_invadersBullet.X-this.pb_Bunker1.Left,_invadersBullet.Y-this.pb_Bunker1.Top,0);
				}
				else if (_invadersBullet.X>=this.pb_Bunker2.Left && _invadersBullet.X<=this.pb_Bunker2.Left+this.pb_Bunker2.Width && _invadersBullet.Y>=this.pb_Bunker2.Top && _invadersBullet.Y<=this.pb_Bunker2.Top+this.pb_Bunker2.Height)
				{
					CheckForBunker(_invadersBullet,_invadersBullet.X-this.pb_Bunker2.Left,_invadersBullet.Y-this.pb_Bunker2.Top,1);
				}
				else if (_invadersBullet.X>=this.pb_Bunker3.Left && _invadersBullet.X<=this.pb_Bunker3.Left+this.pb_Bunker3.Width && _invadersBullet.Y>=this.pb_Bunker3.Top && _invadersBullet.Y<=this.pb_Bunker3.Top+this.pb_Bunker3.Height)
				{
					CheckForBunker(_invadersBullet,_invadersBullet.X-this.pb_Bunker3.Left,_invadersBullet.Y-this.pb_Bunker3.Top,2);
				}
				else if (_invadersBullet.X>=this.pb_Bunker4.Left && _invadersBullet.X<=this.pb_Bunker4.Left+this.pb_Bunker4.Width && _invadersBullet.Y>=this.pb_Bunker4.Top && _invadersBullet.Y<=this.pb_Bunker4.Top+this.pb_Bunker4.Height)
				{
					CheckForBunker(_invadersBullet,_invadersBullet.X-this.pb_Bunker4.Left,_invadersBullet.Y-this.pb_Bunker4.Top,3);
				}
				
				
				if (_invadersBullet.X>=_ship.X && _invadersBullet.X<=_ship.X+this.lb_Ship.Width && _invadersBullet.Y>=_ship.Y && _invadersBullet.Y<=_ship.Y+this.lb_Ship.Height)
				{
					_delayExploded=4;
					_delaySequence=3;
					_shipExploded=true;
					_lives-=1;
					_invadersBullet.Stop();
					_ship.Freeze();
					_invaders.Freeze();
					this.lb_ShipExploded.Left=this.lb_Ship.Left;
					this.lb_ShipExploded.Top=this.lb_Ship.Top-(this.lb_ShipExploded.Height-this.lb_Ship.Height);
					this.lb_ShipExploded.Visible=true;
					_ship.Stop();
					this.lb_ShipsValue.Text=_lives.ToString();

				}
			}
			else if (_random.NextDouble()>_valueForInvadersBullet)
			{
				_invaders.StartInvadersBullet(this._invadersBullet);
				_startedInvadersBullet=true;
				
			}

			
			
			if (_invadersBullet1.IsActive)
			{
				_invadersBullet1.Move();

				if (_invadersBullet1.X>=this.pb_Bunker1.Left && _invadersBullet1.X<=this.pb_Bunker1.Left+this.pb_Bunker1.Width && _invadersBullet1.Y>=this.pb_Bunker1.Top && _invadersBullet1.Y<=this.pb_Bunker1.Top+this.pb_Bunker1.Height)
				{
					CheckForBunker(_invadersBullet1,_invadersBullet1.X-this.pb_Bunker1.Left,_invadersBullet1.Y-this.pb_Bunker1.Top,0);
				}
				else if (_invadersBullet1.X>=this.pb_Bunker2.Left && _invadersBullet1.X<=this.pb_Bunker2.Left+this.pb_Bunker2.Width && _invadersBullet1.Y>=this.pb_Bunker2.Top && _invadersBullet1.Y<=this.pb_Bunker2.Top+this.pb_Bunker2.Height)
				{
					CheckForBunker(_invadersBullet1,_invadersBullet1.X-this.pb_Bunker2.Left,_invadersBullet1.Y-this.pb_Bunker2.Top,1);
				}
				else if (_invadersBullet1.X>=this.pb_Bunker3.Left && _invadersBullet1.X<=this.pb_Bunker3.Left+this.pb_Bunker3.Width && _invadersBullet1.Y>=this.pb_Bunker3.Top && _invadersBullet1.Y<=this.pb_Bunker3.Top+this.pb_Bunker3.Height)
				{
					CheckForBunker(_invadersBullet1,_invadersBullet1.X-this.pb_Bunker3.Left,_invadersBullet1.Y-this.pb_Bunker3.Top,2);
				}
				else if (_invadersBullet1.X>=this.pb_Bunker4.Left && _invadersBullet1.X<=this.pb_Bunker4.Left+this.pb_Bunker4.Width && _invadersBullet1.Y>=this.pb_Bunker4.Top && _invadersBullet1.Y<=this.pb_Bunker4.Top+this.pb_Bunker4.Height)
				{
					CheckForBunker(_invadersBullet1,_invadersBullet1.X-this.pb_Bunker4.Left,_invadersBullet1.Y-this.pb_Bunker4.Top,3);
				}
				
				
				if (_invadersBullet1.X>=_ship.X && _invadersBullet1.X<=_ship.X+this.lb_Ship.Width && _invadersBullet1.Y>=_ship.Y && _invadersBullet1.Y<=_ship.Y+this.lb_Ship.Height)
				{
					_delayExploded=4;
					_delaySequence=3;
					_shipExploded=true;
					_lives-=1;
					_invadersBullet1.Stop();
					_ship.Freeze();
					_invaders.Freeze();
					this.lb_ShipExploded.Left=this.lb_Ship.Left;
					this.lb_ShipExploded.Top=this.lb_Ship.Top-(this.lb_ShipExploded.Height-this.lb_Ship.Height);
					this.lb_ShipExploded.Visible=true;
					_ship.Stop();
					this.lb_ShipsValue.Text=_lives.ToString();

				}
			}
			else if (!_startedInvadersBullet && _random.NextDouble()>_valueForInvadersBullet1)
			{
				_invaders.StartInvadersBullet(this._invadersBullet1);
				
				
			}
			
			if (_shipBullet.IsActive)
			{
				_shipBullet.Move();
				if (_shipBullet.X>=this.pb_Bunker1.Left && _shipBullet.X<=this.pb_Bunker1.Left+this.pb_Bunker1.Width && _shipBullet.Y>=this.pb_Bunker1.Top && _shipBullet.Y<=this.pb_Bunker1.Top+this.pb_Bunker1.Height)
				{
					CheckForBunker(_shipBullet,_shipBullet.X-this.pb_Bunker1.Left,_shipBullet.Y-this.pb_Bunker1.Top,0);
				}
				else if (_shipBullet.X>=this.pb_Bunker2.Left && _shipBullet.X<=this.pb_Bunker2.Left+this.pb_Bunker2.Width && _shipBullet.Y>=this.pb_Bunker2.Top && _shipBullet.Y<=this.pb_Bunker2.Top+this.pb_Bunker2.Height)
				{
					CheckForBunker(_shipBullet,_shipBullet.X-this.pb_Bunker2.Left,_shipBullet.Y-this.pb_Bunker2.Top,1);
				}
				else if (_shipBullet.X>=this.pb_Bunker3.Left && _shipBullet.X<=this.pb_Bunker3.Left+this.pb_Bunker3.Width && _shipBullet.Y>=this.pb_Bunker3.Top && _shipBullet.Y<=this.pb_Bunker3.Top+this.pb_Bunker3.Height)
				{
					CheckForBunker(_shipBullet,_shipBullet.X-this.pb_Bunker3.Left,_shipBullet.Y-this.pb_Bunker3.Top,2);
				}
				else if (_shipBullet.X>=this.pb_Bunker4.Left && _shipBullet.X<=this.pb_Bunker4.Left+this.pb_Bunker4.Width && _shipBullet.Y>=this.pb_Bunker4.Top && _shipBullet.Y<=this.pb_Bunker4.Top+this.pb_Bunker4.Height)
				{
					CheckForBunker(_shipBullet,_shipBullet.X-this.pb_Bunker4.Left,_shipBullet.Y-this.pb_Bunker4.Top,3);
				}
				if (_shipBullet.X>=_invaders.X0 && _shipBullet.X<=_invaders.X1 && _shipBullet.Y>=_invaders.Y0 && _shipBullet.Y<=_invaders.Y1)
				{
					int actualPoints=_invaders.CheckHit(_shipBullet.X,_shipBullet.Y);
					if (actualPoints>0)
					{
						_shipBullet.Stop();
						_points+=actualPoints;
						lb_PointsValue.Text=_points.ToString();
						if (_points>=_pointsToGetNewLive)
						{
							_pointsToGetNewLive+=2000;
							_lives+=1;
							this.lb_ShipsValue.Text=_lives.ToString();
						}
						if (_invaders.IsComplete)
						{
							_ship.Stop();
							_invaders.Stop();
							_ship.Start(_shipXStart,_shipYStart,0,this.Width-lb_Ship.Width,10);
							this._valueForInvadersBullet-=0.02;
							this._valueForInvadersBullet-=0.015;
							if (_invadersDelayValue>6)
							{
								_invadersDelayValue-=2;
							}
							_invaders.Start(_invadersXStart,_invadersYStart,-5,22,10,this.Width-pn_InvadersContainer.Width-40,0,this.Height-pn_InvadersContainer.Height,_invadersDelayValue);
							RestartMilestonesForSpeed();	
							this.pb_Bunker1.Image=_bunkerImageBackup;//.Clone();
							this.pb_Bunker2.Image=_bunkerImageBackup;//.Clone();
							this.pb_Bunker3.Image=_bunkerImageBackup;//.Clone();
							this.pb_Bunker4.Image=_bunkerImageBackup;//.Clone();
				            RestartBunkers();
						}
						else if (_invaders.InvadersToKill==_nextMilestoneToIncreaseSpeed)
						{
							_invaders.Delay-=1;
							lb_DelayValue.Text=_invaders.Delay.ToString();
							if (_milestonesToIncreaseSpeed.Count>0)
							{
								_nextMilestoneToIncreaseSpeed=_milestonesToIncreaseSpeed.Pop();
							}
							else
							{
								_nextMilestoneToIncreaseSpeed=0;
							}

						}
							
					}
				}
			}
			_stillInside=false;
		}
		
	}
}
