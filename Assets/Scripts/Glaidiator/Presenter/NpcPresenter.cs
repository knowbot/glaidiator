using System;
using System.Collections;
using System.Linq;
using Glaidiator.BehaviorTree;
using Glaidiator.Model.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Glaidiator.Presenter
{
    public class NpcPresenter : CharacterPresenter
    {
        // UI
        public TextMeshProUGUI displayState;
        public TextMeshProUGUI displayCooldowns;
        public TextMeshProUGUI staminaWarning;
        public Slider healthBar;
        public Slider staminaBar;

        protected override void Start()
        {
            base.Start();
            provider = GetComponent<AIContainer>();
            if (provider is not AIContainer)
                throw new Exception("Enemy Character Presenter should have an AIContainer as Input Provider");
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            // Register observer methods
            Character.onLowStamina += OnLowStamina;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            // Deregister observer methods
            Character.onLowStamina -= OnLowStamina;
        }


        protected override void LateUpdate()
        {
            // Process inputs and pass them onto the model
            inputs = provider.GetInputs();
            Character.SetInputs(inputs);
            
            // Advance the model
            Character.Tick(Time.deltaTime);
            
            Transform.position = Character.Movement.Position;
            Transform.rotation = Character.Movement.Rotation;
            
            displayState.text = Character.CurrentState.ToString();
            displayCooldowns.text = "";
            foreach (ICooldown cd in Character.Cooldowns.OrderBy(cd => cd.Name))
            {
                displayCooldowns.text += cd.Name + ": " + cd.Cooldown.Duration.ToString("0.00") + "\n";
            }
            UpdateHealth();
            UpdateStamina();
        }

        private void UpdateHealth()
        {
            healthBar.value = Character.Health.Current;
			
        }
		
        private void UpdateStamina()
        {
            staminaBar.value = Character.Stamina.Current;
        }

        private void OnLowStamina()
        {
            StartCoroutine(StaminaWarning());
        }

        private IEnumerator StaminaWarning()
        {
            staminaWarning.text = "Not enough stamina!";
            yield return new WaitForSeconds(1);
            staminaWarning.text = "";
        }
    }
}