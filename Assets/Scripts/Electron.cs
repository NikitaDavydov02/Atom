using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electron : MonoBehaviour {
    // Use this for initialization
    public float q = 1f;
    public float m = 1f;
    public Vector3 speed = new Vector3(0, 0, 0);
    private bool stop = false;
    public bool traectory = true;

    void Start () {
        MainManager.electrons.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
        List<Vector3> forces = new List<Vector3>();
        foreach(Proton proton in MainManager.protons)
        {
            Vector3 vec = proton.transform.position - this.gameObject.transform.position;
            float r = vec.magnitude;
            if (r < 1f)
            {
                stop = true;
                q = 0;
                break;
            }
            float f = (MainManager.k * q * proton.q) / (r * r);
            Vector3 force = vec;
            force *= 10000000;
            force = Vector3.ClampMagnitude(force, f);
            forces.Add(force);
        }
        foreach (Electron electron in MainManager.electrons)
        {
            if(electron.transform.position != this.gameObject.transform.position)
            {
                Vector3 vec = -(electron.transform.position - this.gameObject.transform.position);
                float r = vec.magnitude;
                Vector3 force;
                if (r < 1f)
                {
                    force = -vec;
                    force *= 10000000;
                    force = Vector3.ClampMagnitude(force, 10000000);
                    forces.Add(force);
                }
                else
                {
                    float f = (MainManager.k * q * electron.q) / (r * r);
                    force = vec;
                    force *= 10000000;
                    force = Vector3.ClampMagnitude(force, f);
                    forces.Add(force);
                }
            }
        }
        Vector3 sumForce = new Vector3(0, 0, 0);
        foreach(Vector3 force in forces)
        {
            sumForce += force;
        }
        Vector3 acceleration = sumForce / m;
        Vector3 deltaSpeed = acceleration * Time.deltaTime;
        speed += deltaSpeed;
        Vector3 s = speed * Time.deltaTime;
        if (!stop)
        {
            this.gameObject.transform.Translate(s);
            if (traectory)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                sphere.transform.position = this.gameObject.transform.position;
            }
        }
	}
}
