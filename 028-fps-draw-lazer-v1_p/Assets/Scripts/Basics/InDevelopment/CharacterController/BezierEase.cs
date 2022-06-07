using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierEase : MonoBehaviour
{
    #region Ease enum
    public enum EaseType
    {
       Linear,
       In_Sine,Out_Sine,In_Out_Sine,
       In_Quad,Out_Quad,In_Out_Quad,
       In_Cubic,Out_Cubic,In_Out_Cubic,
       In_Quart,Out_Quart,In_Out_Quart,
       In_Quint,Out_Quint,In_Out_Quint,
       In_Expo,Out_Expo,In_Out_Expo,
       In_Circ,Out_Circ,In_Out_Circ,
       In_Elastic,Out_Elastic,In_Out_Elastic,
       In_Back,Out_Back,In_Out_Back,
       In_Bounce,Out_Bounce,In_Out_Bounce,
       Flash,In_Flash,Out_Flash,In_Out_Flash,
       
    };
    #endregion
    public EaseType EaseTypes;

    [SerializeField] private int SelectedEaseNo;
    
    private EaseType SelectedType;

    public bool UseCustomEase = false;
    private bool UseCustomEaseRecord;
    
    [Header(" F(x) = A*x^Q + B*x^W + C  ")] 
    
    [SerializeField] private float Q_ReferenceEquationParabolaFactor=3; 
    [SerializeField] private float W_ReferenceEquationParabolaFactor=2;                                    
    
    [SerializeField] private float A_ReferenceEquationParameter=1;  
    [SerializeField] private float B_ReferenceEquationParameter=-2;  
    [SerializeField]  private float C_ReferenceEquationParameter=6;  
    
    [SerializeField] private float Reference_x_StartPoint=0;  
    [SerializeField] private float Reference_x_EndPoint=2.5f;  
    
    private float Q_Prime=3; 
    private float W_Prime=2;                                    
    
    private float A_Prime=1;  
    private float B_Prime=-2;  
    private float C_Prime=6;  
    
    private float Reference_x_Start_Prime=0;  
    private float Reference_x_End_Prime=2.5f;  
    
    /*
    
   
     */
    #region EQUATIONS

    private bool listAreTaken = false;
    private List<float> Q_List;
    private List<float> W_List;
    private List<float> A_List;
    private List<float> B_List;
    private List<float> C_List;
    private List<float> S_List;
    private List<float> E_List;
    
    // Linear
    
    private float Q_1=3; 
    private float W_1=2;                                    
    
    private float A_1=1;  
    private float B_1=-2;  
    private float C_1=6;  
    
    private float S_1 =0;  
    private float E_1 =2.5f;  
    
    //--In_Sine ----------------------------------------
    
    private float Q_2=3; 
    private float W_2=2;                                    
    
    private float A_2=1;  
    private float B_2=-2;  
    private float C_2=6;  
    
    private float S_2 =0;   
    private float E_2 =2.5f;  
    
    //--Out_Sine
    
    private float Q_3=3; 
    private float W_3=2;                                    
    
    private float A_3=1;  
    private float B_3=-2;  
    private float C_3=6;  
    
    private float S_3 =0;  
    private float E_3 =2.5f;  
    
    //--In_Out_Sine
    
    private float Q_4=3; 
    private float W_4=2;                                    
    
    private float A_4=1;  
    private float B_4=-2;  
    private float C_4=6;  
    
    private float S_4 =0;  
    private float E_4 =2.5f;  
    
    //--In_Quad ----------------------------------------
    
    private float Q_5=3; 
    private float W_5=2;                                     
    
    private float A_5=1;  
    private float B_5=-2;  
    private float C_5=6;  
    
    private float S_5 =0;  
    private float E_5 =2.5f;  
    
    //--Out_Quad
    
    private float Q_6=3;  
    private float W_6=2;                                     
    
    private float A_6=1;  
    private float B_6=-2;   
    private float C_6=6;  
    
    private float S_6 =0;  
    private float E_6 =2.5f;   
    
    //--In_Out_Quad
    
    private float Q_7=3; 
    private float W_7=2;                                    
    
    private float A_7=1;  
    private float B_7=-2;   
    private float C_7=6;  
    
    private float S_7 =0;   
    private float E_7 =2.5f;  
    
    //--In_Cubic ---------------------------------------- 
   
    private float Q_8=3; 
    private float W_8=2;                                    
    
    private float A_8=1;  
    private float B_8=-2;  
    private float C_8=6;  
    
    private float S_8 =0;  
    private float E_8 =2.5f;  
  
    //--Out_Cubic
   
    private float Q_9=3;  
    private float W_9=2;                                     
    
    private float A_9=1;  
    private float B_9=-2;  
    private float C_9=6;   
    
    private float S_9 =0;  
    private float E_9 =2.5f;  
   
    //-- In_Out_Cubic 
   
    private float Q_10=3; 
    private float W_10=2;                                    
    
    private float A_10=1;  
    private float B_10=-2;  
    private float C_10=6;  
    
    private float S_10 =0;  
    private float E_10 =2.5f;  
    
    //-- In_Quart ---------------------------------------- 

    private float Q_11=3; 
    private float W_11=2;                                    
    
    private float A_11=1;  
    private float B_11=-2;  
    private float C_11=6;  
    
    private float S_11 =0;  
    private float E_11 =2.5f;  
    
    //--Out_Quart
    
    private float Q_12=3; 
    private float W_12=2;                                    
    
    private float A_12=1;  
    private float B_12=-2;  
    private float C_12=6;  
    
    private float S_12 =0;// SET VALUE 
    private float E_12 =2.5f;// SET VALUE 
    
    //-- In_Out_Quart
    private float Q_13=1 ;// SET VALUE
    private float W_13=2;// SET VALUE                                   
    
    private float A_13=1;// SET VALUE 
    private float B_13=-2;// SET VALUE 
    private float C_13=6;// SET VALUE 
    
    private float S_13 =0;// SET VALUE 
    private float E_13 =2.5f;// SET VALUE  
    
    //--    In_Quint ----------------------------------------
   
    private float Q_14=3;// SET VALUE
    private float W_14=2;// SET VALUE                                   
    
    private float A_14=1;// SET VALUE 
    private float B_14=-2;// SET VALUE 
    private float C_14=6;// SET VALUE 
    
    private float S_14 =0;// SET VALUE 
    private float E_14 =2.5f;// SET VALUE 
    
    //--Out_Quint
    
    private float Q_15=3;// SET VALUE
    private float W_15=2;// SET VALUE                                   
    
    private float A_15=1;// SET VALUE 
    private float B_15=-2;// SET VALUE 
    private float C_15=6;// SET VALUE 
    
    private float S_15=0;// SET VALUE 
    private float E_15=2.5f;// SET VALUE 
    
   
    
    //--,In_Out_Quint
    
    private float Q_16=3;// SET VALUE
    private float W_16=2;// SET VALUE                                   
    
    private float A_16=1;// SET VALUE 
    private float B_16=-2;// SET VALUE 
    private float C_16=6;// SET VALUE 
    
    private float S_16 =0;// SET VALUE 
    private float E_16 =2.5f;// SET VALUE 
    
    //-- In_Expo ---------------------------------------- 
   
    private float Q_17=3; 
    private float W_17=2;// SET VALUE                                   
    
    private float A_17=1;// SET VALUE 
    private float B_17=-2;// SET VALUE 
    private float C_17=6;// SET VALUE 
    
    private float S_17 =0;// SET VALUE 
    private float E_17 =2.5f;// SET VALUE 

    //--Out_Expo
    
    private float Q_18=3;// SET VALUE
    private float W_18=2;// SET VALUE                                   
    
    private float A_18=1;// SET VALUE 
    private float B_18=-2;// SET VALUE 
    private float C_18=6;// SET VALUE 
    
    private float S_18 =0;// SET VALUE 
    private float E_18 =2.5f;// SET VALUE 
    
    //--In_Out_Expo
    
    private float Q_19=3;// SET VALUE
    private float W_19=2;// SET VALUE                                   
    
    private float A_19=1;// SET VALUE 
    private float B_19=-2;// SET VALUE 
    private float C_19=6;// SET VALUE 
    
    private float S_19 =0;// SET VALUE 
    private float E_19 =2.5f;// SET VALUE 
    
    //-- In_Circ ---------------------------------------- 
    
    private float Q_20=3;// SET VALUE
    private float W_20=2;// SET VALUE                                   
    
    private float A_20=1;// SET VALUE 
    private float B_20=-2;// SET VALUE 
    private float C_20=6;// SET VALUE 
    
    private float S_20 =0;// SET VALUE 
    private float E_20 =2.5f;// SET VALUE 
    
    //--Out_Circ

    private float Q_21=3;// SET VALUE
    private float W_21=2;// SET VALUE                                   
    
    private float A_21=1;// SET VALUE 
    private float B_21=-2;// SET VALUE 
    private float C_21=6;// SET VALUE 
    
    private float S_21 =0;// SET VALUE 
    private float E_21 =2.5f;// SET VALUE 
    
    //--In_Out_Circ
    
    private float Q_22=3;// SET VALUE
    private float W_22=2;// SET VALUE                                   
    
    private float A_22=1;// SET VALUE 
    private float B_22=-2;// SET VALUE 
    private float C_22=6;// SET VALUE 
    
    private float S_22 =0;// SET VALUE 
    private float E_22 =2.5f;// SET VALUE 
    
    //-- In_Elastic ---------------------------------------- 
   
    private float Q_23=3;// SET VALUE
    private float W_23=2;// SET VALUE                                   
    
    private float A_23=1;// SET VALUE 
    private float B_23=-2;// SET VALUE 
    private float C_23=6;// SET VALUE 
    
    private float S_23 =0;// SET VALUE 
    private float E_23 =2.5f;// SET VALUE 
    
    //--Out_Elastic
  
    private float Q_24=3;// SET VALUE
    private float W_24=2;// SET VALUE                                   
    
    private float A_24=1;// SET VALUE 
    private float B_24=-2;// SET VALUE 
    private float C_24=6;// SET VALUE 
    
    private float S_24 =0;// SET VALUE 
    private float E_24 =2.5f;// SET VALUE 
    
    //--In_Out_Elastic
    
    private float Q_25=3;// SET VALUE
    private float W_25=2;// SET VALUE                                   
    
    private float A_25=1;// SET VALUE 
    private float B_25=-2;// SET VALUE 
    private float C_25=6;// SET VALUE 
    
    private float S_25 =0;// SET VALUE 
    private float E_25 =2.5f;// SET VALUE 
    
    //-- In_Back ---------------------------------------- 
    
    private float Q_26=3;// SET VALUE
    private float W_26=2;// SET VALUE                                   
    
    private float A_26=1;// SET VALUE 
    private float B_26=-2;// SET VALUE 
    private float C_26=6;// SET VALUE 
    
    private float S_26 =0;// SET VALUE 
    private float E_26 =2.5f;// SET VALUE 
    
    //--Out_Back
    
    private float Q_27=3;// SET VALUE
    private float W_27=2;// SET VALUE                                   
    
    private float A_27=1;// SET VALUE 
    private float B_27=-2;// SET VALUE 
    private float C_27=6;// SET VALUE 
    
    private float S_27 =0;// SET VALUE 
    private float E_27 =2.5f;// SET VALUE 
    
    //--In_Out_Back

    
    private float Q_28=3;// SET VALUE
    private float W_28=2;// SET VALUE                                   
    
    private float A_28=1;// SET VALUE 
    private float B_28=-2;// SET VALUE 
    private float C_28=6;// SET VALUE 
    
    private float S_28 =0;// SET VALUE 
    private float E_28 =2.5f;// SET VALUE 
    
    //--  In_Bounce ---------------------------------------- 
    
    private float Q_29=3;// SET VALUE
    private float W_29=2;// SET VALUE                                   
    
    private float A_29=1;// SET VALUE 
    private float B_29=-2;// SET VALUE 
    private float C_29=6;// SET VALUE 
    
    private float S_29 =0;// SET VALUE 
    private float E_29 =2.5f;// SET VALUE 
    
    //--Out_Bounce
    
    private float Q_30=3;// SET VALUE
    private float W_30=2;// SET VALUE                                   
    
    private float A_30=1;// SET VALUE 
    private float B_30=-2;// SET VALUE 
    private float C_30=6;// SET VALUE 
    
    private float S_30 =0;// SET VALUE 
    private float E_30 =2.5f;// SET VALUE 
    
    //--In_Out_Bounce
    
    private float Q_31=3;// SET VALUE
    private float W_31=2;// SET VALUE                                   
    
    private float A_31=1;// SET VALUE 
    private float B_31=-2;// SET VALUE 
    private float C_31=6;// SET VALUE 
    
    private float S_31 =0;// SET VALUE 
    private float E_31 =2.5f;// SET VALUE 
    
    //--Flash------------------------------------------
    
    private float Q_32=3;// SET VALUE
    private float W_32=2;// SET VALUE                                   
    
    private float A_32=1;// SET VALUE 
    private float B_32=-2;// SET VALUE 
    private float C_32=6;// SET VALUE 
    
    private float S_32 =0;// SET VALUE 
    private float E_32 =2.5f;// SET VALUE 
    
    //--In_Flash
    
    private float Q_33=3;// SET VALUE
    private float W_33=2;// SET VALUE                                   
    
    private float A_33=1;// SET VALUE 
    private float B_33=-2;// SET VALUE 
    private float C_33=6;// SET VALUE 
    
    private float S_33=0;// SET VALUE 
    private float E_33 =2.5f;// SET VALUE 
    
    //--Out_Flash
    
    private float Q_34=3;// SET VALUE
    private float W_34=5;// SET VALUE                                   
    
    private float A_34=1;// SET VALUE 
    private float B_34=-2;// SET VALUE 
    private float C_34=6;// SET VALUE 
    
    private float S_34 =0;// SET VALUE 
    private float E_34 =2.5f;// SET VALUE 
    
    //--In_Out_Flash
    
    private float Q_35 =3;// SET VALUE
    private float W_35=2;// SET VALUE                                   
    
    private float A_35=1;// SET VALUE 
    private float B_35=-2;// SET VALUE 
    private float C_35=6;// SET VALUE 
    
    private float S_35=0;// SET VALUE 
    private float E_35 =2.5f;// SET VALUE 

    #endregion
    
      private void setEaseValues()
    {
        SelectedEaseNo=1;
        switch (EaseTypes)
        {
            
                
            case EaseType.Linear :
                SelectedEaseNo = 1;
                break;
            case EaseType.In_Sine :
                SelectedEaseNo = 2;
                break;
            case EaseType.Out_Sine :
                SelectedEaseNo = 3;
                break;
            case EaseType.In_Out_Sine :
                SelectedEaseNo = 4;
                break;
            case EaseType.In_Quad:
                SelectedEaseNo = 5;
                break;
            case EaseType.Out_Quad:
                SelectedEaseNo = 6;
                break;
            case EaseType.In_Out_Quad :
                SelectedEaseNo = 7;
                break;
            case EaseType.In_Cubic :
                SelectedEaseNo = 8;
                break;
            case EaseType.Out_Cubic :
                SelectedEaseNo = 9;
                break;
            case EaseType.In_Out_Cubic :
                SelectedEaseNo = 10;
                break;
            case EaseType.In_Quart :
                SelectedEaseNo = 11;
                break;
            case EaseType.Out_Quart :
                SelectedEaseNo = 12;
                break;
            case EaseType.In_Out_Quart :
                SelectedEaseNo = 13;
                break;
            case EaseType.In_Quint:
                SelectedEaseNo = 14;
                break;
            case EaseType.Out_Quint :
                SelectedEaseNo = 15;
                break;
            case EaseType.In_Out_Quint :
                SelectedEaseNo = 16;
                break;
            case EaseType.In_Expo:
                SelectedEaseNo = 17;
                break;
            case EaseType.Out_Expo :
                SelectedEaseNo = 18;
                break;
            case EaseType.In_Out_Expo :
                SelectedEaseNo = 19;
                break;
            case EaseType.In_Circ :
                SelectedEaseNo = 20;
                break;
            case EaseType.Out_Circ:
                SelectedEaseNo = 21;
                break;
            case EaseType.In_Out_Circ:
                SelectedEaseNo = 22;
                break;
            case EaseType.In_Elastic :
                SelectedEaseNo = 23;
                break;
            case EaseType.Out_Elastic :
                SelectedEaseNo = 24;
                break;
            case EaseType.In_Out_Elastic :
                SelectedEaseNo = 25;
                break;
            case EaseType.In_Back :
                SelectedEaseNo = 26;
                break;
            case EaseType.Out_Back :
                SelectedEaseNo = 27;
                break;
            case EaseType.In_Out_Back :
                SelectedEaseNo = 28;
                break;
            case EaseType.In_Bounce :
                SelectedEaseNo = 29;
                break;
            case EaseType.Out_Bounce :
                SelectedEaseNo = 30;
                break;
            case EaseType.In_Out_Bounce :
                SelectedEaseNo = 31;
                break;
            case EaseType.Flash :
                SelectedEaseNo = 32;
                break;
            case EaseType.In_Flash :
                SelectedEaseNo = 33;
                break;
            case EaseType.Out_Flash :
                SelectedEaseNo = 34;
                break;
            case EaseType.In_Out_Flash :
                SelectedEaseNo = 35;
                break;
           
            
            default: SelectedEaseNo = 1;
                break;

        }
        
        Q_Prime=Q_1=Q_List[SelectedEaseNo]; 
        W_Prime=W_1=W_List[SelectedEaseNo];                                    
    
        A_Prime=A_1=A_List[SelectedEaseNo];
        B_Prime = B_List[SelectedEaseNo];
        C_Prime=C_List[SelectedEaseNo];  
    
        Reference_x_Start_Prime=S_List[SelectedEaseNo];  
        Reference_x_End_Prime=E_List[SelectedEaseNo];  
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckTypeChange();
    }

    private void CheckTypeChange()
    {
        if (UseCustomEase!=UseCustomEaseRecord)
        {
            UseCustomEaseRecord = UseCustomEase;
            if (UseCustomEase)
            {
                Q_Prime=Q_ReferenceEquationParabolaFactor ; 
                W_Prime=W_ReferenceEquationParabolaFactor;                                    
    
                A_Prime=A_ReferenceEquationParameter;  
                B_Prime=B_ReferenceEquationParameter;  
                C_Prime=C_ReferenceEquationParameter;  
    
                Reference_x_Start_Prime=Reference_x_StartPoint;  
                Reference_x_End_Prime=Reference_x_EndPoint;
     
            }
            else
            {
                setEaseValues();
            }
            
        }
        if (SelectedType != EaseTypes&& !UseCustomEase)
        {
            SelectedType = EaseTypes;

            setEaseValues();
            
        }
    }

  
    public List<float> GetEquationSettings()
    {
        if (!listAreTaken)
        {
            listAreTaken = true;
            TakeLists();
            CheckTypeChange();
        }
        
        
        
        List <float> result =new List<float>() ; 
        result.Add(Q_Prime);
        result.Add(W_Prime);
        result.Add(B_Prime);
        result.Add(Q_Prime);
        result.Add(C_Prime);
        result.Add(Reference_x_Start_Prime);
        result.Add(Reference_x_End_Prime);

        return result;
    }

    
    //---------------------------------------
    private void TakeLists()
    {
        //-----------------------Q_List
        Q_List.Add(Q_1);
        Q_List.Add(Q_2);
        Q_List.Add(Q_3);
        Q_List.Add(Q_4);
        Q_List.Add(Q_5);
        Q_List.Add(Q_6);
        Q_List.Add(Q_7);
        Q_List.Add(Q_8);
        Q_List.Add(Q_9);
        Q_List.Add(Q_10);
        Q_List.Add(Q_11);
        Q_List.Add(Q_12);
        Q_List.Add(Q_13);
        Q_List.Add(Q_14);
        Q_List.Add(Q_15);
        Q_List.Add(Q_16);
        Q_List.Add(Q_17);
        Q_List.Add(Q_18);
        Q_List.Add(Q_19);
        Q_List.Add(Q_20);
        Q_List.Add(Q_21);
        Q_List.Add(Q_22);
        Q_List.Add(Q_23);
        Q_List.Add(Q_24);
        Q_List.Add(Q_25);
        Q_List.Add(Q_26);
        Q_List.Add(Q_27);
        Q_List.Add(Q_28);
        Q_List.Add(Q_29);
        Q_List.Add(Q_30);
        Q_List.Add(Q_31);
        Q_List.Add(Q_32);
        Q_List.Add(Q_33);
        Q_List.Add(Q_34);
        Q_List.Add(Q_35);

        //-----------------------W_List
        W_List.Add(W_1);
        W_List.Add(W_2);
        W_List.Add(W_3);
        W_List.Add(W_4);
        W_List.Add(W_5);
        W_List.Add(W_6);
        W_List.Add(W_7);
        W_List.Add(W_8);
        W_List.Add(W_9);
        W_List.Add(W_10);
        W_List.Add(W_11);
        W_List.Add(W_12);
        W_List.Add(W_13);
        W_List.Add(W_14);
        W_List.Add(W_15);
        W_List.Add(W_16);
        W_List.Add(W_17);
        W_List.Add(W_18);
        W_List.Add(W_19);
        W_List.Add(W_20);
        W_List.Add(W_21);
        W_List.Add(W_22);
        W_List.Add(W_23);
        W_List.Add(W_24);
        W_List.Add(W_25);
        W_List.Add(W_26);
        W_List.Add(W_27);
        W_List.Add(W_28);
        W_List.Add(W_29);
        W_List.Add(W_30);
        W_List.Add(W_31);
        W_List.Add(W_32);
        W_List.Add(W_33);
        W_List.Add(W_34);
        W_List.Add(W_35);
        
        
        //-----------------------A_List
        A_List.Add(A_1);
        A_List.Add(A_2);
        A_List.Add(A_3);
        A_List.Add(A_4);
        A_List.Add(A_5);
        A_List.Add(A_6);
        A_List.Add(A_7);
        A_List.Add(A_8);
        A_List.Add(A_9);
        A_List.Add(A_10);
        A_List.Add(A_11);
        A_List.Add(A_12);
        A_List.Add(A_13);
        A_List.Add(A_14);
        A_List.Add(A_15);
        A_List.Add(A_16);
        A_List.Add(A_17);
        A_List.Add(A_18);
        A_List.Add(A_19);
        A_List.Add(A_20);
        A_List.Add(A_21);
        A_List.Add(A_22);
        A_List.Add(A_23);
        A_List.Add(A_24);
        A_List.Add(A_25);
        A_List.Add(A_26);
        A_List.Add(A_27);
        A_List.Add(A_28);
        A_List.Add(A_29);
        A_List.Add(A_30);
        A_List.Add(A_31);
        A_List.Add(A_32);
        A_List.Add(A_33);
        A_List.Add(A_34);
        A_List.Add(A_35);
        //-----------------------B_List
        B_List.Add(B_1);
        B_List.Add(B_2);
        B_List.Add(B_3);
        B_List.Add(B_4);
        B_List.Add(B_5);
        B_List.Add(B_6);
        B_List.Add(B_7);
        B_List.Add(B_8);
        B_List.Add(B_9);
        B_List.Add(B_10);
        B_List.Add(B_11);
        B_List.Add(B_12);
        B_List.Add(B_13);
        B_List.Add(B_14);
        B_List.Add(B_15);
        B_List.Add(B_16);
        B_List.Add(B_17);
        B_List.Add(B_18);
        B_List.Add(B_19);
        B_List.Add(B_20);
        B_List.Add(B_21);
        B_List.Add(B_22);
        B_List.Add(B_23);
        B_List.Add(B_24);
        B_List.Add(B_25);
        B_List.Add(B_26);
        B_List.Add(B_27);
        B_List.Add(B_28);
        B_List.Add(B_29);
        B_List.Add(B_30);
        B_List.Add(B_31);
        B_List.Add(B_32);
        B_List.Add(B_33);
        B_List.Add(B_34);
        B_List.Add(B_35);
        //-----------------------C_List
        C_List.Add(C_1);
        C_List.Add(C_2);
        C_List.Add(C_3);
        C_List.Add(C_4);
        C_List.Add(C_5);
        C_List.Add(C_6);
        C_List.Add(C_7);
        C_List.Add(C_8);
        C_List.Add(C_9);
        C_List.Add(C_10);
        C_List.Add(C_11);
        C_List.Add(C_12);
        C_List.Add(C_13);
        C_List.Add(C_14);
        C_List.Add(C_15);
        C_List.Add(C_16);
        C_List.Add(C_17);
        C_List.Add(C_18);
        C_List.Add(C_19);
        C_List.Add(C_20);
        C_List.Add(C_21);
        C_List.Add(C_22);
        C_List.Add(C_23);
        C_List.Add(C_24);
        C_List.Add(C_25);
        C_List.Add(C_26);
        C_List.Add(C_27);
        C_List.Add(C_28);
        C_List.Add(C_29);
        C_List.Add(C_30);
        C_List.Add(C_31);
        C_List.Add(C_32);
        C_List.Add(C_33);
        C_List.Add(C_34);
        C_List.Add(C_35);
        //-----------------------S_List
        S_List.Add(S_1);
        S_List.Add(S_2);
        S_List.Add(S_3);
        S_List.Add(S_4);
        S_List.Add(S_5);
        S_List.Add(S_6);
        S_List.Add(S_7);
        S_List.Add(S_8);
        S_List.Add(S_9);
        S_List.Add(S_10);
        S_List.Add(S_11);
        S_List.Add(S_12);
        S_List.Add(S_13);
        S_List.Add(S_14);
        S_List.Add(S_15);
        S_List.Add(S_16);
        S_List.Add(S_17);
        S_List.Add(S_18);
        S_List.Add(S_19);
        S_List.Add(S_20);
        S_List.Add(S_21);
        S_List.Add(S_22);
        S_List.Add(S_23);
        S_List.Add(S_24);
        S_List.Add(S_25);
        S_List.Add(S_26);
        S_List.Add(S_27);
        S_List.Add(S_28);
        S_List.Add(S_29);
        S_List.Add(S_30);
        S_List.Add(S_31);
        S_List.Add(S_32);
        S_List.Add(S_33);
        S_List.Add(S_34);
        S_List.Add(S_35);
        //-----------------------E_List
        E_List.Add(E_1);
        E_List.Add(E_2);
        E_List.Add(E_3);
        E_List.Add(E_4);
        E_List.Add(E_5);
        E_List.Add(E_6);
        E_List.Add(E_7);
        E_List.Add(E_8);
        E_List.Add(E_9);
        E_List.Add(E_10);
        E_List.Add(E_11);
        E_List.Add(E_12);
        E_List.Add(E_13);
        E_List.Add(E_14);
        E_List.Add(E_15);
        E_List.Add(E_16);
        E_List.Add(E_17);
        E_List.Add(E_18);
        E_List.Add(E_19);
        E_List.Add(E_20);
        E_List.Add(E_21);
        E_List.Add(E_22);
        E_List.Add(E_23);
        E_List.Add(E_24);
        E_List.Add(E_25);
        E_List.Add(E_26);
        E_List.Add(E_27);
        E_List.Add(E_28);
        E_List.Add(E_29);
        E_List.Add(E_30);
        E_List.Add(E_31);
        E_List.Add(E_32);
        E_List.Add(E_33);
        E_List.Add(E_34);
        E_List.Add(E_35);
    }
    
}
/*
 
 [System.Serializable]
    
    public struct CustomEase {
        
        public List<string > EaseTypes;
        
    }
    [SerializeField] private List<CustomEase> EaseList =new List<CustomEase> () ;
    
    
    
 
 */