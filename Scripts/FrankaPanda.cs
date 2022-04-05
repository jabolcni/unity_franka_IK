using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FrankaPanda : MonoBehaviour
{

    [DllImport("FrankaPandaDLL", EntryPoint = "?franka_IK@@YAXPEAN0N0_N1@Z")]
    public static extern void franka_IK([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] double[] q, double[] TF, double q7, double[] q0, bool limit, bool flange);
    
    [DllImport("FrankaPandaDLL", EntryPoint = "?franka_IK_f@@YAXPEAM0M0_N1@Z")]
    public static extern void franka_IK_f([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] float[] q, float[] TF, float q7, float[] q0, bool limit, bool flange);    

    private GameObject Base;
	private GameObject J1;
    private GameObject J2;
    private GameObject J3;
    private GameObject J4;
    private GameObject J5;
    private GameObject J6;
    private GameObject J7;
	private GameObject TCP;
	private GameObject finger1;
	private GameObject finger2;
	public GameObject target;

    float q1_start = 0.0f, q2_start = 30.0f, q3_start = 0.0f, q4_start = -60.0f, q5_start = 0.0f, q6_start = 90.0f, q7_start = 0.0f;

    public float[] q;
    double[] qq = new double[7];
    float[] qMax;
    float[] qMin;
    float finger_pos;

    // Start is called before the first frame update
    void Start()
    {

		Base = GameObject.Find("Panda_Base");
        J1 = GameObject.Find("Panda_J1");
        J2 = GameObject.Find("Panda_J2");
        J3 = GameObject.Find("Panda_J3");
        J4 = GameObject.Find("Panda_J4");
        J5 = GameObject.Find("Panda_J5");
        J6 = GameObject.Find("Panda_J6");
        J7 = GameObject.Find("Panda_J7");
		TCP  = GameObject.Find("TCP_fingers");
		finger1  = GameObject.Find("finger1");
		finger2  = GameObject.Find("finger2");
        q = new float[] { q1_start, q2_start, q3_start, q4_start, q5_start, q6_start, q7_start };
        qMax = new float[] { 166.003062f, 101.0010001f, 166.003062f, -3.99925f, 166.0031f, 215.0024f, 166.0031f };
        qMin = new float[] { -166.003062f, -101.0010001f, -166.003062f, -176.0012f, -166.0031f, -1.00268f, -166.0031f };
        finger_pos = 0.03f;

        //initial values
        
        J1.transform.localEulerAngles = new Vector3(0, 0, -q[0]);
        J2.transform.localEulerAngles = new Vector3(90, 0, -q[1]);
        J3.transform.localEulerAngles = new Vector3(-90, 0, -q[2]);
        J4.transform.localEulerAngles = new Vector3(-90, 0, -q[3]);
        J5.transform.localEulerAngles = new Vector3(90, 0, -q[4]);
        J6.transform.localEulerAngles = new Vector3(-90, 0, -q[5]);
        J7.transform.localEulerAngles = new Vector3(-90, 0, -q[6]);

        finger1.transform.localPosition = new Vector3(0, finger_pos, -0.0448f);
        finger2.transform.localPosition = new Vector3(0, -finger_pos, -0.0448f);
    }

    // Update is called once per frame
    void Update()
    {
        //close the program on escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
 
        double q7 = (double)(45*Mathf.Deg2Rad);
        double[] q0 = {(double)(q[0]*Mathf.Deg2Rad),(double)(q[1]*Mathf.Deg2Rad),(double)(q[2]*Mathf.Deg2Rad),(double)(q[3]*Mathf.Deg2Rad),(double)(q[4]*Mathf.Deg2Rad),(double)(q[5]*Mathf.Deg2Rad),(double)(q[6]*Mathf.Deg2Rad)};
        float[] q_old = q;
		
        Matrix4x4 lh_m = Matrix4x4.identity;
        lh_m = Base.transform.worldToLocalMatrix*target.transform.localToWorldMatrix;
        Quaternion lh_quat = lh_m.rotation;
        Quaternion rh_quat = new Quaternion(lh_quat.x, -lh_quat.y, lh_quat.z, -lh_quat.w);
        Matrix4x4 matrix = Matrix4x4.identity;
        matrix.SetTRS(new Vector3(lh_m[0,3], -lh_m[1,3], lh_m[2,3]), rh_quat, new Vector3(1, 1, 1));
        double[] TF ={(double)matrix[0],(double)matrix[1],(double)matrix[2],(double)matrix[3],(double)matrix[4],(double)matrix[5],(double)matrix[6],(double)matrix[7],(double)matrix[8],(double)matrix[9],(double)matrix[10],(double)matrix[11],(double)matrix[12],(double)matrix[13],(double)matrix[14],(double)matrix[15]};

        franka_IK(qq, TF, q7, q0, true, false);

        q = new float[] {(float)(qq[0]*Mathf.Rad2Deg),(float)(qq[1]*Mathf.Rad2Deg),(float)(qq[2]*Mathf.Rad2Deg),(float)(qq[3]*Mathf.Rad2Deg),(float)(qq[4]*Mathf.Rad2Deg),(float)(qq[5]*Mathf.Rad2Deg),(float)(qq[6]*Mathf.Rad2Deg)};
        if (float.IsNaN(q[0]) || float.IsNaN(q[1]) || float.IsNaN(q[2]) || float.IsNaN(q[3]) || float.IsNaN(q[4]) || float.IsNaN(q[5]) || float.IsNaN(q[6]))
        {
            q = q_old;
        }
        
        
        /* You can also limit the joints in C# script
 		for (int i = 0; i < 7; i++)
		{
			if (q[i] > qMax[i])
			{
				q[i] = qMax[i];
			}
			if (q[i] < qMin[i])
			{
				q[i] = qMin[i];
			}
		}
        */

        J1.transform.localEulerAngles = new Vector3(0, 0, -q[0]);
        J2.transform.localEulerAngles = new Vector3(90, 0, -q[1]);
        J3.transform.localEulerAngles = new Vector3(-90, 0, -q[2]);
        J4.transform.localEulerAngles = new Vector3(-90, 0, -q[3]);
        J5.transform.localEulerAngles = new Vector3(90, 0, -q[4]);
        J6.transform.localEulerAngles = new Vector3(-90, 0, -q[5]);
        J7.transform.localEulerAngles = new Vector3(-90, 0, -q[6]);

        finger1.transform.localPosition = new Vector3(0, finger_pos, -0.0448f);
        finger2.transform.localPosition = new Vector3(0, -finger_pos, -0.0448f);       
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
		
        for (int i = 0; i < 7; i++)
        {
            GUI.Label(new Rect(10, i * 30, 0, 0), "q" + (i + 1) + "= " + System.Math.Round(q[i], 2) + "°", style);
        }
    } 
	
}