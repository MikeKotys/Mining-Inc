using GreenPandaAssets.Scripts.Services;

namespace GreenPandaAssets.Scripts.Other
{
	/// <summary>Handles quarry upgrade logic.</summary>
	public class QuaryUpgradable : AUpgradable
	{
		event QuaryUpgradedEventHandler QuaryUpgradedEvent;

		void RaiseQuaryUpgradedEvent()
		{
			QuaryUpgradedEvent?.Invoke(this, new QuaryUpgradedEventArgs(_level, _maxLevel));
		}

		public void SubscribeToQuaryUpgradedEvent(QuaryUpgradedEventHandler handler)
		{
			QuaryUpgradedEvent += handler;
		}

		public void UnsubscribeFromQuaryUpgradedEvent(QuaryUpgradedEventHandler handler)
		{
			QuaryUpgradedEvent -= handler;
		}

		public override void Upgrade()
		{
			base.Upgrade();

			RaiseQuaryUpgradedEvent();
		}
	}
}
