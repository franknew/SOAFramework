using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace SOAFramework.Library
{
    /**/
    /// <summary>
    /// ��̬��ֵ
    /// </summary>
    public class Evaluator
    {
        /**/
        /// <summary>
        /// ������,������ʽ�������׳��쳣
        /// </summary>
        /// <param name="statement">���ʽ,��"1+2+3+4"</param>
        /// <returns>���</returns>
        public static object Eval(string statement)
        {
            if (statement.Trim() != string.Empty)
            {
                Evaluator evaluator = new Evaluator();
                return evaluator.GetFormulaResult(statement);
            }
            else
            {
                return null;
            }
        }


        private object GetFormulaResult(string s)
        {
            if (s == "")
            {
                return null;
            }
            string S = BuildingRPN(s);

            string tmp = "";
            System.Collections.Stack sk = new System.Collections.Stack();

            char c = ' ';
            System.Text.StringBuilder Operand = new System.Text.StringBuilder();
            double x, y;
            for (int i = 0;
                i < S.Length;
                i++)
            {
                c = S[i];
                //added c==',' for germany culture
                if (char.IsDigit(c) || c == '.' || c == ',')
                {
                    //����ֵ�ռ�.
                    Operand.Append(c);
                }
                else if (c == ' ' && Operand.Length > 0)
                {
                    #region ������ת��
                    try
                    {
                        tmp = Operand.ToString();
                        if (tmp.StartsWith("-"))//������ת��һ��ҪС��...������ֱ��֧��.
                        {
                            //�����ҵ��㷨�������֧������Զ���ᱻִ��.
                            sk.Push(-((double)Convert.ToDouble(tmp.Substring(1, tmp.Length - 1))));
                        }
                        else
                        {
                            sk.Push(Convert.ToDouble(tmp));
                        }
                    }
                    catch
                    {
                        return null; //
                    }
                    Operand = new System.Text.StringBuilder();
                    #endregion
                }
                else if (c == '+'//���������.˫Ŀ���㴦��.
                    || c == '-'
                    || c == '*'
                    || c == '/'
                    || c == '%'
                    || c == '^')
                {
                    #region ˫Ŀ����
                    if (sk.Count > 0)/*�������ı��ʽ����û�а��������.���Ǹ������ǿմ�.������߼�����������.*/
                    {
                        y = (double)sk.Pop();
                    }
                    else
                    {
                        sk.Push(0);
                        break;
                    }
                    if (sk.Count > 0)
                        x = (double)sk.Pop();
                    else
                    {
                        sk.Push(y);
                        break;
                    }
                    switch (c)
                    {
                        case '+':
                            sk.Push(x + y);
                            break;
                        case '-':
                            sk.Push(x - y);
                            break;
                        case '*':
                            if (y == 0)
                            {
                                sk.Push(x * 1);
                            }
                            else
                            {
                                sk.Push(x * y);
                            }
                            break;
                        case '/':
                            if (y == 0)
                            {
                                sk.Push(x / 1);
                            }
                            else
                            {
                                sk.Push(x / y);
                            }
                            break;
                        case '%':
                            sk.Push(x % y);
                            break;
                        case '^'://
                            if (x > 0)//
                            {
                                //��ԭ������,�������������Ǹ���,��Ҫ��������η�ʱ��δ��������.�������뻹�����˰�.
                                sk.Push(System.Math.Pow(x, y));
                                //
                            }
                            //
                            else//
                            {
                                //
                                double t = y;
                                //
                                string ts = "";
                                //
                                t = 1 / (2 * t);
                                //
                                ts = t.ToString();
                                //
                                if (ts.ToUpper().LastIndexOf('E') > 0)//
                                {
                                    //
                                    ;
                                    //
                                }
                                //
                            }
                            break;
                    }
                    #endregion
                }
                else if (c == '!')//��Ŀȡ��. )
                {
                    sk.Push(-((double)sk.Pop()));
                }
            }
            if (sk.Count > 1)
            {
                return null;//;
            }
            if (sk.Count == 0)
            {
                return null;//;
            }
            return sk.Pop();
        }
        /**/
        /// <summary>
        /// 
        /// </summary>
        private string BuildingRPN(string s)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(s);
            System.Collections.Stack sk = new System.Collections.Stack();
            System.Text.StringBuilder re = new System.Text.StringBuilder();

            char c = ' ';
            //sb.Replace( " ","" );
            //һ��ʼ,��ֻȥ���˿ո�.�����Ҳ��벻֧�ֺ����ͳ������˵���ȫOUT��.
            for (int i = 0;
                i < sb.Length;
                i++)
            {
                c = sb[i];
                //added c==',' for german culture
                if (char.IsDigit(c) || c == ',')//���ֵ�ȻҪ��.
                    re.Append(c);
                //if( char.IsWhiteSpace( c )||
                char.IsLetter(c);//����ǿհ�,��ô��Ҫ.������ĸҲ��Ҫ.
                //continue;
                switch (c)//����������ַ�...�г���Ҫ,û���г��Ĳ�Ҫ.
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '%':
                    case '^':
                    case '!':
                    case '(':
                    case ')':
                    case '.':
                        re.Append(c);
                        break;
                    default:
                        continue;
                }
            }
            sb = new System.Text.StringBuilder(re.ToString());
            #region �Ը��Ž���Ԥת�崦��.���ű䵥Ŀ�������.
            for (int i = 0; i < sb.Length - 1; i++)
                if (sb[i] == '-' && (i == 0 || sb[i - 1] == '('))
                    sb[i] = '!';
            //�ַ�ת��.
            #endregion
            #region ����׺���ʽ��Ϊ��׺���ʽ.
            re = new System.Text.StringBuilder();
            for (int i = 0;
                i < sb.Length;
                i++)
            {
                if (char.IsDigit(sb[i]) || sb[i] == '.')//�������ֵ.
                {
                    re.Append(sb[i]);
                    //�����׺ʽ
                }
                else if (sb[i] == '+'
                    || sb[i] == '-'
                    || sb[i] == '*'
                    || sb[i] == '/'
                    || sb[i] == '%'
                    || sb[i] == '^'
                    || sb[i] == '!')//.
                {
                    #region ���������
                    while (sk.Count > 0) //ջ��Ϊ��ʱ
                    {
                        c = (char)sk.Pop();
                        //��ջ�еĲ���������.
                        if (c == '(') //�������������.ͣ.
                        {
                            sk.Push(c);
                            //��������������ѹ��.��Ϊ����������Ҫ����ƥ��.
                            break;
                            //�ж�.
                        }
                        else
                        {
                            if (Power(c) < Power(sb[i]))//������ȼ����ϴεĸ�,��ѹջ.
                            {
                                sk.Push(c);
                                break;
                            }
                            else
                            {
                                re.Append(' ');
                                re.Append(c);
                            }
                            //�������������,��ô�������������׺ʽ��.
                        }
                    }
                    sk.Push(sb[i]);
                    //���²�������ջ.
                    re.Append(' ');
                    #endregion
                }
                else if (sb[i] == '(')//�������ȼ�����
                {
                    sk.Push('(');
                    re.Append(' ');
                }
                else if (sb[i] == ')')//�������ȼ��µ�
                {
                    while (sk.Count > 0) //ջ��Ϊ��ʱ
                    {
                        c = (char)sk.Pop();
                        //pop Operator
                        if (c != '(')
                        {
                            re.Append(' ');
                            re.Append(c);
                            //����ո���Ҫ��Ϊ�˷�ֹ����ɵ��������ٲ�����������.
                            re.Append(' ');
                        }
                        else
                            break;
                    }
                }
                else
                    re.Append(sb[i]);
            }
            while (sk.Count > 0)//�������һ����ջ��.
            {
                re.Append(' ');
                re.Append(sk.Pop());
            }
            #endregion
            re.Append(' ');
            return FormatSpace(re.ToString());
            //���������һ�α��ʽ��ʽ��.������Ǻ�׺ʽ��.  
        }

        /// <summary>  
        /// ���ȼ�����Ժ���.  
        /// </summary>  
        /// <param name="opr"></param>  
        /// <returns></returns>  
        private static int Power(char opr)
        {
            switch (opr)
            {
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                    return 2;
                case '%':
                case '^':
                case '!':
                    return 3;
                default:
                    return 0;
            }
        }

        /// <summary>  
        /// �淶���沨�����ʽ.
        /// </summary>  
        /// <param name="s"></param>  
        /// <returns></returns>  
        private static string FormatSpace(string s)
        {
            System.Text.StringBuilder ret = new System.Text.StringBuilder();
            for (int i = 0;
                i < s.Length;
                i++)
            {
                if (!(s.Length > i + 1 && s[i] == ' ' && s[i + 1] == ' '))
                    ret.Append(s[i]);
                else
                    ret.Append(s[i]);
            }
            return ret.ToString();
            //.Replace( '!','-' );
        }
    }
}