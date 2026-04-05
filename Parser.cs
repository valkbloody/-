using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using фапра.States;

namespace фапра
{
    public class Parser
    {
        /// <summary>
        /// Класс Парсер
        /// </summary>
        private enum Vn { Z, A, T, L, I, J, E, P, R, M, N, X, B, O, END, ERR } // нетерминальные символы
        
        private Vn cur_state = Vn.Z; // текущее состяние

        private int cur_lexem_id = -1; // индекс текущей ликсемы

        private List<Lexema> list_lexems; // список всех лексем

        private Lexema cur_lexem; // текущая лексема

        private Error_page errors = new Error_page(); // список ошибок
        public Error_page Errors 
        {
            get { return errors; }
        }
 
      
        private bool ExcpectedError(string exp_str)
        {
            if (!CANgetnext)
            {
                errors.addError($"Ожидалось {exp_str}, но не было получено", -1, 0, cur_lexem.location);
                return true;
            }
            return false;
        }
        // осн. функция парсера
        public void Parse(List<Lexema> lexems)
        {
            list_lexems = lexems;
            getnext();

            while (true)
            {
                if (ExcpectedError("func")) break;
                Start s = new Start(cur_lexem.name.ToLower(), cur_lexem.location);

                if (s.Parse("func") == 1)
                {
                    errors.addErrors(s.Errors.path, s.Errors.line, s.Errors.column, s.Errors.message);
                    getnext();
                    break;
                }
                else if (cur_lexem.id == 8 || cur_lexem.id == 1 || cur_lexem.id == 12)
                {
                    errors.addError($"Ожидалось func, но не было получено", -1, 0, cur_lexem.location);
                    break;
                }

                getnext();
            }

            while (true)
            {
                if (ExcpectedError("<")) break;
                OpenBrcPars opbep = new OpenBrcPars(cur_lexem.name, cur_lexem.location);
                if (opbep.Parse() == 1)
                {
                    getnext();
                    break;
                }
                else
                {
                    if (cur_lexem.id == 1 || cur_lexem.id == 12 || cur_lexem.id == 9)
                    {
                        errors.addError($"Ожидалось <, но не было получено", -1, 0, cur_lexem.location);
                        break;
                    }
                    errors.addErrors(opbep.Errors.path, opbep.Errors.line, opbep.Errors.column, opbep.Errors.message);
                    getnext();
                }
            }



            int pars_end = 0;
            Error_page err = new Error_page("", -1, 0, "");
            while (true)
            {

                while (true)
                {

                    if (ExcpectedError("int"))
                    {
                        pars_end = 2;
                        break;
                    }
                    IntState intst = new IntState(cur_lexem.name.ToLower(), cur_lexem.location);
                    if (intst.Parse("int") == 1)
                    {
                        errors.addErrors(intst.Errors.path, intst.Errors.line, intst.Errors.column, intst.Errors.message);
                        getnext();
                        break;
                    }
                    else
                    {
                        if (cur_lexem.id == 3 || cur_lexem.id == 9 || cur_lexem.id == 4)
                        {
                            err.message[0] += cur_lexem.name;
                            pars_end = 2;
                            if (cur_lexem.id == 9)
                            {
                                getnext();
                                pars_end = 3;
                            }
                            break;
                        }
                        errors.addErrors(intst.Errors.path, intst.Errors.line, intst.Errors.column, intst.Errors.message);
                        getnext();
                    }
                }

                if (pars_end > 0) break;
                while (true)
                {
                    if (ExcpectedError(", или >"))
                    {
                        pars_end = 2;
                        break;
                    }
                    ParsEnums parsen = new ParsEnums(cur_lexem.name, cur_lexem.location);
                    int parsed = parsen.Parse();
                    if (parsed == 1 || parsed == 2)
                    {
                        if (parsed == 2) pars_end = 1;
                        getnext();
                        break;
                    }
                    else
                    {
                        if (cur_lexem.id == 3 || cur_lexem.id == 4)
                        {
                            err.message[0] += cur_lexem.name;
                            pars_end = 1;
                            break;
                        }
                        errors.addErrors(parsen.Errors.path, parsen.Errors.line, parsen.Errors.column, parsen.Errors.message);
                        getnext();
                    }
                }
                if (pars_end > 0) break;
            }

            err.path[0] = cur_lexem.location;
            if (pars_end == 2 || pars_end == 3)
            {
                if (err.message[0] != string.Empty) errors.addError($"Ожидалось int, встречено: " + err.message[0], -1, 0, err.path[0]);
                if (pars_end == 2) errors.addError($"Ожидалось >, встречено: " + err.message[0], -1, 0, err.path[0]);
                err.message[0] = String.Empty;
                pars_end = 1;
            }
            if (pars_end == 1)
            {
                while (true)
                {
                    if (ExcpectedError("идентификатор")) break;
                    if (cur_lexem.id == 3)
                    {
                        if (err.message[0] != string.Empty) errors.addError($"Ожидалось >, встречено: " + err.message[0], -1, 0, err.path[0]);
                        getnext();
                        break;
                    }
                    else
                    {
                        if (cur_lexem.id == 4 || cur_lexem.id == 6)
                        {
                            errors.addError($"Ожидалось идентификатор, но не было получено", -1, 0, cur_lexem.location);
                            break;
                        }
                        err.message[0] += cur_lexem.name;
                        getnext();
                    }
                }
            }
            err.message[0]  = "";
            while (true)
            {
                if (ExcpectedError("=")) break;
                EqualState equal = new EqualState(cur_lexem.name, cur_lexem.location);
                if (equal.Parse() == 1)
                {
                    getnext();
                    break;
                }
                else
                {
                    if (cur_lexem.id == 6)
                    {
                        errors.addError($"Ожидалось =, но не было получено", -1, 0, cur_lexem.location);
                        break;
                    }
                    err.message[0] += cur_lexem.name;
                    getnext();
                }
            }
            if (err.message[0] != string.Empty) errors.addError($"Ожидалось =, отброшенный фрагмент: " + err.message[0], -1, 0, err.path[0]);
            while (true)
            {
                if (ExcpectedError("(")) break;
                OpenBrc_IDS opbepIDS = new OpenBrc_IDS(cur_lexem.name, cur_lexem.location);
                if (opbepIDS.Parse() == 1)
                {
                    getnext();
                    break;
                }
                else
                {
                    if (cur_lexem.id == 3 || cur_lexem.id == 12)
                    {
                        errors.addError($"Ожидалось (, но не было получено", -1, 0, cur_lexem.location);
                        break;
                    }
                    errors.addErrors(opbepIDS.Errors.path, opbepIDS.Errors.line, opbepIDS.Errors.column, opbepIDS.Errors.message);
                    getnext();
                }
            }
            int pars_end_IDS = 0;
            Error_page err_Pars = new Error_page("", -1, 0, "");
            while (true)
            {
                while (true)
                {
                    if (ExcpectedError("идентификатор"))
                    {
                        pars_end_IDS = 1;
                        break;
                    }
                    if (cur_lexem.id == 3)
                    {
                        getnext();
                        break;
                    }
                    else
                    {
                        if (cur_lexem.id == 5 || cur_lexem.id == 4 || cur_lexem.id == 9)
                        {
                            errors.addError($"Ожидалось идентификатор, но не было получено", -1, 0, cur_lexem.location);
                            pars_end_IDS = 1;
                            break;
                        }
                        errors.addError($"Ожидалось идентификатор, но не было получено", -1, 0, cur_lexem.location);
                        if (cur_lexem.id != 7) getnext();
                        break;
                    }
                }
                if (pars_end_IDS > 0) break;
                while (true)
                {
                    if (ExcpectedError(", или )"))
                        {
                            pars_end_IDS = 1;
                            break;
                        }
                    IDSParsEnums parsen = new IDSParsEnums(cur_lexem.name, cur_lexem.location);
                    int parsed = parsen.Parse();
                    if (parsed == 1 || parsed == 2)
                    {
                        if (parsed == 2) pars_end_IDS = 1;
                        getnext();
                        break;
                    }
                    else
                    {
                        if (cur_lexem.id == 5 || cur_lexem.id == 4 || cur_lexem.id == 9)
                        {
                            errors.addError($"Ожидалось , или ), но не было получено", -1, 0, cur_lexem.location);
                            if (cur_lexem.id == 4 || cur_lexem.id == 9)      pars_end_IDS = 1;
                            else pars_end_IDS = 2;
                            getnext();
                            break;
                        }
                        errors.addErrors(parsen.Errors.path, parsen.Errors.line, parsen.Errors.column, parsen.Errors.message);
                        getnext();
                    }
                }
                if (pars_end_IDS > 0) break;
            }


            if (pars_end_IDS == 1) 
            {
                int key = 0;
                while (true)
                {
                    if (ExcpectedError("=>")) break;
                    if (cur_lexem.id == 5)
                    {
                        getnext();
                        break;
                    }
                    else
                    {
                        if (cur_lexem.id == 3)
                        {
                            if (key == 0) errors.addError($"Ожидалось =>, но встречено: " + cur_lexem.name, -1, 0, cur_lexem.location);
                            break;
                        }
                        key = 1;
                        errors.addError($"Ожидалось =>, но встречено: "+cur_lexem.name, -1, 0, cur_lexem.location);
                        getnext();
                    }
                }
            }
            ArithOpesState arith_op = new ArithOpesState(cur_lexem.name.ToLower(), cur_lexem.location);
            while (true)
            {
                arith_op.arith_oper(cur_lexem.id,cur_lexem.name, cur_lexem.location);
                if (cur_lexem.id == 13) break;
                getnext();

            }
            errors.addErrors(arith_op.Errors.path, arith_op.Errors.line, arith_op.Errors.column, arith_op.Errors.message);
           
        }
        public void addError(string buf, string location)
        {
            errors.addError(buf, -1, 0, location);
        }
        private void getnext()
        {
            cur_lexem_id++;
            cur_lexem = list_lexems[cur_lexem_id];
        }
        private bool CANgetnext
        {
            get => cur_lexem_id < list_lexems.Count-1;
        }

    }
}
