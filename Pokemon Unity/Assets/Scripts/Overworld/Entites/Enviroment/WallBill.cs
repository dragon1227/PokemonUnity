﻿using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Environment
{
	public class WallBill : Entity
	{
		protected override float CalculateCameraDistance(Vector3 CPosition)
		{
			return base.CalculateCameraDistance(CPosition) - 0.4f;
		}

		public override void UpdateEntity()
		{
			if (this.Rotation.y != Game.Camera.Yaw)
			{
				this.Rotation.y = Game.Camera.Yaw;
				CreatedWorld = false;
			}

			base.UpdateEntity();
		}

		public override void Render()
		{
			//this.Draw(this.Model, Textures, false);
		}
	}
}