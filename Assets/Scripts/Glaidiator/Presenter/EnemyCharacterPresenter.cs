﻿using System;
using System.Collections;
using System.Linq;
using BehaviorTree;
using Glaidiator.Model.Actions;
using Glaidiator.Model.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Glaidiator.Presenter
{
    public class EnemyCharacterPresenter : CharacterPresenter
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


        protected override void Update()
        {
            base.Update();
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