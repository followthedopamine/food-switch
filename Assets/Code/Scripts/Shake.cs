using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {

  private Vector3 initialPosition;
  private Camera cam;
  private Vector3 initialCameraPosition;

  private float amount = 3.0f; //how much it shakes

  void Start() {
    initialPosition = gameObject.transform.position;
    cam = Camera.main;
    initialCameraPosition = cam.transform.position;
  }

  void Update() {
    gameObject.transform.position = new Vector3(
      initialPosition.x + Random.Range(-amount, amount) * Time.deltaTime,
      initialPosition.y + Random.Range(-amount, amount) * Time.deltaTime,
    0);
    cam.transform.position = new Vector3(
      initialCameraPosition.x + Random.Range(-amount, amount) * Time.deltaTime,
      initialCameraPosition.y + Random.Range(-amount, amount) * Time.deltaTime,
    initialCameraPosition.z);
  }

  void OnDisable() {
    cam.transform.position = initialCameraPosition;
  }
}
