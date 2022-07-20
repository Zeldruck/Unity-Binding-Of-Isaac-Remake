using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] B;
    public GameObject[] BL;
    public GameObject[] BR;
    public GameObject[] F;
    public GameObject[] L;
    public GameObject[] LR;
    public GameObject[] R;
    public GameObject[] T;
    public GameObject[] TB;
    public GameObject[] TL;
    public GameObject[] TR;
    public GameObject[] BLR;
    public GameObject[] LBT;
    public GameObject[] RBT;
    public GameObject[] TLR;
    [Space]
    public GameObject[] bosses;
    
    public GameObject GetGoodRoom(SDoors _doors)
    {
        if (_doors.B && !_doors.R && !_doors.L && !_doors.T)
        {
            return B[Random.Range(0, B.Length)];
        }
        if (_doors.B && _doors.R && !_doors.L && !_doors.T)
        {
            return BR[Random.Range(0, BR.Length)];
        }
        if (_doors.B && !_doors.R && _doors.L && !_doors.T)
        {
            return BL[Random.Range(0, BL.Length)];
        }
        if (_doors.B && !_doors.R && !_doors.L && _doors.T)
        {
            return TB[Random.Range(0, TB.Length)];
        }
        if (_doors.B && _doors.R && _doors.L && !_doors.T)
        {
            return BLR[Random.Range(0, BLR.Length)];
        }
        if (_doors.B && !_doors.R && _doors.L && _doors.T)
        {
            return LBT[Random.Range(0, LBT.Length)];
        }
        if (_doors.B && _doors.R && !_doors.L && _doors.T)
        {
            return RBT[Random.Range(0, RBT.Length)];
        }
        
        if (_doors.R && !_doors.B && !_doors.L && !_doors.T)
        {
            return R[Random.Range(0, R.Length)];
        }
        if (_doors.R && !_doors.B && _doors.L && !_doors.T)
        {
            return LR[Random.Range(0, LR.Length)];
        }
        if (_doors.R && !_doors.B && !_doors.L && _doors.T)
        {
            return TR[Random.Range(0, TR.Length)];
        }
        if (_doors.R && !_doors.B && _doors.L && _doors.T)
        {
            return TLR[Random.Range(0, TLR.Length)];
        }
        
        if (_doors.L && !_doors.B && !_doors.R && !_doors.T)
        {
            return L[Random.Range(0, L.Length)];
        }
        if (_doors.L && !_doors.B && !_doors.R && _doors.T)
        {
            return TL[Random.Range(0, TL.Length)];
        }
        
        if (_doors.T && !_doors.B && !_doors.R && !_doors.L)
        {
            return T[Random.Range(0, T.Length)];
        }
        
        if (_doors.T && _doors.B && _doors.R && _doors.L)
        {
            return F[Random.Range(0, F.Length)];
        }

        return null;
    }
}
