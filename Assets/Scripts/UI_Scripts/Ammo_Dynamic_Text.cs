using TMPro;
using UnityEngine;

public class Ammo_Dynamic_Text : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private AttackerHandler _playerAmmo;

    private void Awake()
    {
        Invoke("SetStartAmmoValues", 0.3f);

        _playerAmmo.OnShoot += UpdateAmmoText;
    }

    private void OnDestroy()
    {
        _playerAmmo.OnShoot -= UpdateAmmoText;
    }

    private void SetStartAmmoValues()
    {
        _ammoText.text = $"{_playerAmmo.CurrentMagazine}/{_playerAmmo.MaxMagazine}";
    }

    private void UpdateAmmoText(int ammoValue)
    {
        _ammoText.text = $"{ammoValue}/{_playerAmmo.MaxMagazine}";
    }
}
