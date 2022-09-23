using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MatrixMxN
{
    int _m;
    public int m => _m;
    int _n;
    public int n => _n;

    float[][] _entries;
    public float[][] entries => _entries;

    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < _m; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                result += _entries[i][j].ToString();
                result += " ";
            }
            result += "\n";
        }
        return result;
    }

    public MatrixMxN Clone()
    {
        MatrixMxN clone = new MatrixMxN(_m, _n);
        for (int i = 0; i < _m; i++)
        {
            for (int j = 0; j < _n; j++)
            {
                clone.entries[i][j] = _entries[i][j];
            }
        }
        return clone;
    }

    public MatrixMxN(int m, int n)
    {
        _m = m;
        _n = n;

        _entries = new float[m][];
        for (int i = 0; i < m; i++)
        { _entries[i] = new float[n]; }
    }

    public float Get(int r_idx, int c_idx)
    { return _entries[r_idx][c_idx]; }

    public void Set(int r_idx, int c_idx, float entry)
    { _entries[r_idx][c_idx] = entry; }

    public float[] GetRow(int idx)
    { return (float[])_entries[idx].Clone(); }

    public float[] GetCol(int idx)
    {
        float[] col = new float[_n];
        for (int i = 0; i < _m; i++)
        { col[i] = _entries[_m][idx]; }
        return col;
    }

    public void SetRow(int idx, float[] row)
    {
        row.CopyTo(_entries[idx], 0);
    }

    public void SetCol(int idx, float[] col)
    {
        for (int i = 0; i < _m; i++)
        { _entries[i][idx] = col[i]; }
    }

    public void SwapRows(int idx_a, int idx_b)
    {
        float[] temp = _entries[idx_a];
        _entries[idx_a] = _entries[idx_b];
        _entries[idx_b] = temp;
    }

    public void ScaleRow(int idx, float l)
    {
        for (int i = 0; i < _n; i++)
        { _entries[idx][i] *= l; }
    }

    public void OffsetRow(int idx, float[] offsets)
    {
        for (int i = 0; i < _n; i++)
        { _entries[idx][i] += offsets[i]; }
    }

    public void NormalizeRow(int idx)
    {
        for (int i = 0; i < _n; i++)
        {
            float l = Mathf.Abs(_entries[idx][i]);
            if (l > Mathf.Epsilon)
            {
                for (int j = i; j < _n; j++)
                { _entries[idx][j] /= l; }
                break;
            }
        }
    }

    public MatrixMxN Multiply(MatrixMxN B)
    {
        MatrixMxN result = new MatrixMxN(_m, B.n);

        // Check for compatibility
        if (_n != B.m)
        { return result; }

        for(int a_r = 0; a_r < _m; a_r++)
        {
            for(int b_c = 0; b_c < B.n; b_c++)
            {
                float dot = 0;
                for(int i = 0; i < _n; i++)
                { dot += _entries[a_r][i] * B.Get(i, b_c); }
                result.Set(a_r, b_c, dot);
            }
        }

        return result;
    }

    public static MatrixMxN RREF(MatrixMxN A)
    {
        MatrixMxN R = A.Clone();
        int row_idx = 0;
        int pvt_idx = 0;
        List<int> pvt_idx_list = new List<int>();

        // REF
        while (row_idx < R.m && pvt_idx < R.n)
        {
            // zero-col check
            bool zeroed = true;
            for (int i = 0; i < R.m; i++)
            {
                if (Mathf.Abs(R.Get(i, pvt_idx)) > Mathf.Epsilon)
                {
                    zeroed = false;
                    break;
                }
            }
            if (zeroed)
            { pvt_idx++; }

            // zero-entry check
            float col_lead = R.Get(row_idx, pvt_idx);
            int col_lead_idx = row_idx;
            if (Mathf.Abs(col_lead) < Mathf.Epsilon)
            {
                for(int i = 0; i < R.m; i++)
                {
                    float cand = R.Get(i, pvt_idx);
                    if(cand > col_lead)
                    {
                        col_lead = cand;
                        col_lead_idx = i;
                    }
                }
            }
            R.SwapRows(row_idx, col_lead_idx);

            // enforce a leading one
            R.NormalizeRow(row_idx);

            // eliminate
            for(int i = row_idx+1; i < R.m; i++)
            {
                float mult = R.Get(i, pvt_idx) / R.Get(row_idx, pvt_idx);
                float[] offsets = R.GetRow(row_idx);
                for(int j = 0; j < R.n; j++)
                { offsets[j] *= -mult; }
                R.OffsetRow(i, offsets);
            }

            // record
            pvt_idx_list.Add(pvt_idx);
            row_idx++;
            pvt_idx++;
        }

        // RREF
        // backstep elimination
        for (int i = 0; i < pvt_idx_list.Count; i++)
        {
            row_idx = i;
            pvt_idx = pvt_idx_list[i];
            
            for(int j = 0; j < row_idx; j++)
            {
                float mult = R.Get(j, pvt_idx) / R.Get(row_idx, pvt_idx);
                float[] offsets = R.GetRow(row_idx);
                for (int k = 0; k < R.n; k++)
                { offsets[k] *= -mult; }
                R.OffsetRow(j, offsets);
            }
        }
        // normalization
        for (int i = 0; i < pvt_idx_list.Count; i++)
        {
            row_idx = i;
            pvt_idx = pvt_idx_list[i];

            R.ScaleRow(row_idx, 1 / R.Get(row_idx, pvt_idx));
        }

        return R;
    }
}
