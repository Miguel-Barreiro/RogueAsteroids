using Configuration;
using UnityEngine;

namespace View
{
	public class OnAnimationEnd : StateMachineBehaviour
	{
		public AnimationType AnimationType;

		[SerializeField]
		private bool DetectOnExit = true;
        
		private bool _running;
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			base.OnStateEnter(animator, stateInfo, layerIndex);
			if (!DetectOnExit) {
				_running = true;
			}
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			base.OnStateUpdate(animator, stateInfo, layerIndex);

			if (stateInfo.normalizedTime > 0.95f && _running && !DetectOnExit) {
				_running = false;
				TriggerAnimationEnd(animator);
			}
		}

		override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			if (DetectOnExit) {
				TriggerAnimationEnd(animator);
			}
		}
        
		private void TriggerAnimationEnd(Animator animator) {
			EndAnimationController endAnimationController = animator.GetComponent<EndAnimationController>();
			if (endAnimationController != null) {
				endAnimationController.AnimationEnd(AnimationType);
			} else {
				Debug.LogError($"expected <EndAnimationController> component in gameobject {animator.gameObject.name}");
			}
		}

	}}