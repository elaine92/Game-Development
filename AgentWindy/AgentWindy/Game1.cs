#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace AgentWindy
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		//Initialize the neccasary variables
		Texture2D agentTex, dragonTex, healthMushroomTex, poisonMushroomTex, fireballTex, bulletTex, backgroundTex, lifeTex;
		Texture2D startScreen, gameOverScreen, youWonScreen;

		Rectangle agent, dragon, bullet;
		Rectangle healthMushroom1, healthMushroom2;
		Rectangle poisonMushroom1, poisonMushroom2, poisonMushroom3;
		Rectangle fireball;

		Vector2 background;
		Vector2 timeRemain, countDownPosition;

		int fireTime, startTime, secondPassed, timeRemains;
		int agentLife = 5;
		int dragonLife = 15;

		int lifeBucket = 0;

		int playerRunningSpeed = 2;

		string gameState = "Start";

		KeyboardState keyState;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            	

			graphics.PreferredBackBufferHeight = 580;
			graphics.PreferredBackBufferWidth = 1024;
		}


		protected override void Initialize ()
		{
			base.Initialize ();
		}


		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			//Declare Assets
			lifeTex = Content.Load<Texture2D> ("Textures\\Life");

			agentTex = Content.Load<Texture2D> ("Textures\\windy");
			dragonTex = Content.Load<Texture2D> ("Textures\\dragon");
			healthMushroomTex = Content.Load<Texture2D> ("Textures\\healthy_mushroom");
			poisonMushroomTex = Content.Load<Texture2D> ("Textures\\poisonus_mushroom");
			fireballTex = Content.Load<Texture2D> ("Textures\\fireball");
			bulletTex = Content.Load<Texture2D> ("Textures\\bullet");

			backgroundTex = Content.Load<Texture2D> ("Screens\\background");
			startScreen = Content.Load<Texture2D> ("Screens\\start");
			youWonScreen = Content.Load<Texture2D> ("Screens\\won");
			gameOverScreen = Content.Load<Texture2D> ("Screens\\go");

			//Declare Positions and sizes of the assets
			agent = new Rectangle(50, 320, 50, 75);
			dragon = new Rectangle(850, 290, 80, 105);
			fireball = new Rectangle(850, 330, 30, 30);
			bullet = new Rectangle(50, 330, 30, 30);

			healthMushroom1 = new Rectangle(500, 10, 40, 40);
			healthMushroom2 = new Rectangle(700, 20, 40, 40);

			poisonMushroom1 = new Rectangle(300, 30, 40, 40);
			poisonMushroom2 = new Rectangle(400, 30, 40, 40);
			poisonMushroom3 = new Rectangle(600, 30, 40, 40);

			background = new Vector2(0.0f, 0.0f);
			timeRemain = new Vector2 (800.0f, 50.0f);
			countDownPosition = new Vector2 (50.0f, 50.0f);

			startTime = 180 * 1000;
			secondPassed = 0;
			timeRemains = 0;
		}


		protected override void Update (GameTime gameTime)
		{

			keyState = Keyboard.GetState ();

			if (gameState == "Start" && keyState.IsKeyDown (Keys.Space)) 
			{
				gameState = "Running";
			}

			if (keyState.IsKeyDown (Keys.Left)) 
			{
				agent.X -= playerRunningSpeed;
				bullet.X = agent.X;
			}

			if (keyState.IsKeyDown(Keys.Right))
			{
				agent.X += playerRunningSpeed;
				bullet.X = agent.X;
			}

			if (keyState.IsKeyDown(Keys.Up))
			{
				agent.Y -= playerRunningSpeed;
				bullet.Y = agent.Y;
			}

			if (keyState.IsKeyDown(Keys.Down))
			{
				agent.Y += playerRunningSpeed;
				bullet.Y = agent.Y;
			}

			fireball.X -= 2;
			if (fireTime % 500 == 0)
			{
				fireball = new Rectangle(850, 380, 30, 30);
			}

			healthMushroom1.Y += 2;
			healthMushroom2.Y += 3;
			poisonMushroom1.Y += 4;
			poisonMushroom2.Y += 3;
			poisonMushroom3.Y += 2;

			if (fireTime % 2200 == 0) 
			{
				healthMushroom1 = new Rectangle (500, 10, 60, 60);
				healthMushroom2 = new Rectangle (700, 20, 60, 60);
				poisonMushroom1 = new Rectangle (300, 30, 60, 60);
				poisonMushroom2 = new Rectangle (400, 30, 60, 60);
				poisonMushroom3 = new Rectangle (600, 30, 60, 60);
			}

			if (fireTime % 3200 == 0)
			{
				healthMushroom1 = new Rectangle(500, 10, 60, 60);
				healthMushroom2 = new Rectangle(700, 20, 60, 60);
			}

			fireTime += gameTime.ElapsedGameTime.Milliseconds;
			secondPassed += gameTime.ElapsedGameTime.Milliseconds;
			timeRemains = startTime - fireTime;

			if (timeRemains < 0)
			{
				this.Exit();
			}

			if (keyState.IsKeyDown(Keys.S))
			{
				bullet.X += 10;
			}

			if (fireball.Intersects(bullet))
			{
				bullet.X = agent.X;
				fireball = new Rectangle(850, 380, 30, 30);
			}

			if (bullet.Intersects(dragon))
			{
				dragonLife = dragonLife - 1;
				//dragonLife--;
				bullet = new Rectangle(850, 380, 30, 30);
				if (dragonLife <= 0)
				{
					gameState = "Won";
				} 
			}

			if (agent.Intersects(healthMushroom1) ||
				agent.Intersects(healthMushroom2))
			{
				lifeBucket++;
				healthMushroom1 = new Rectangle(500, 10, 40, 40);
				healthMushroom2 = new Rectangle(700, 20, 40, 40);
			}

			if (lifeBucket == 10)
			{
				agentLife++;
				lifeBucket = 0;
			}

			if (agent.Intersects(fireball))
			{
				agentLife--;
				if (agentLife <= 0)
				{
					gameState = "Over";
				}


			}

			if (agent.Intersects(poisonMushroom1) ||
				agent.Intersects(poisonMushroom2) ||
				agent.Intersects(poisonMushroom3))
			{
				agentLife--;
				playerRunningSpeed = 0;
				poisonMushroom1 = new Rectangle(300, 30, 40, 40);
				poisonMushroom2 = new Rectangle(400, 30, 40, 40);
				poisonMushroom3 = new Rectangle(600, 30, 40, 40);
			}




			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			//graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
		
			//TODO: Add your drawing code here
			if (gameState == "Start")
			{
				spriteBatch.Begin();
				spriteBatch.Draw(startScreen, new Vector2(0, 0), Color.White);
				spriteBatch.End();
			}

			if (gameState == "Running")
			{
				spriteBatch.Begin();
				spriteBatch.Draw(backgroundTex, background, Color.White);

				//spriteBatch.DrawString(life, "Agent Lifes : " + agentLife.ToString(), timeRemain, Color.Black);
				//spriteBatch.DrawString(life, "Dragon Lifes :" + dragonLife.ToString(), new Vector2(800.0f, 100.0f), Color.Black);
				//spriteBatch.DrawString(counDown, timeRemains.ToString(), countDownPosition, Color.Red);
				//spriteBatch.DrawString(life, "Millyseconds is to defeat the Dragon..!!!", new Vector2(50.0f, 120.0f), Color.Black);
					
				spriteBatch.Draw(bulletTex, bullet, Color.White); 
				spriteBatch.Draw(agentTex, agent, Color.White); 
				spriteBatch.Draw(dragonTex, dragon, Color.White); 
				spriteBatch.Draw(fireballTex, fireball, Color.White);

				spriteBatch.Draw(healthMushroomTex, healthMushroom1, Color.White);
				spriteBatch.Draw(healthMushroomTex, healthMushroom2, Color.White);
				spriteBatch.Draw(poisonMushroomTex, poisonMushroom1, Color.White);
				spriteBatch.Draw(poisonMushroomTex, poisonMushroom2, Color.White);
				spriteBatch.Draw(poisonMushroomTex, poisonMushroom3, Color.White);
				spriteBatch.End();

										
			}	

			if (gameState == "Won")
			{
				spriteBatch.Begin();
				spriteBatch.Draw(youWonScreen, new Vector2(0, 0), Color.White);
				spriteBatch.End();
			}

			if (gameState == "Over")
			{
				spriteBatch.Begin();
				spriteBatch.Draw(gameOverScreen, new Vector2(0, 0), Color.White);
				spriteBatch.End();	
			}
      
			base.Draw (gameTime);
		}
	}
}

