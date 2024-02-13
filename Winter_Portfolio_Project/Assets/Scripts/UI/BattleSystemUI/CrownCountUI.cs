using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WPP.Battle.UI
{
    public class CrownCountUI : MonoBehaviour
    {
        [SerializeField] private CrownSystem _player;
        [SerializeField] private CrownSystem _opponent;

        [SerializeField] private TextMeshProUGUI _playerText;
        [SerializeField] private TextMeshProUGUI _opponentText;

        private void OnEnable()
        {
            _player.OnCrownCountChange += SetPlayerCrownCount;
            _opponent.OnCrownCountChange += SetOpponentCrownCount;

            SetOpponentCrownCount(_opponent.CrownCount);
            SetPlayerCrownCount(_player.CrownCount);
        }

        private void SetOpponentCrownCount(int count)
        {
            _opponentText.text = count.ToString();
        }

        private void SetPlayerCrownCount(int count)
        {
            _playerText.text = count.ToString();
        }
    }
}
