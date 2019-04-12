using ClassToSql.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassToSql
{
    public class JoinTable<F>
    {
        public JoinTable(SqlPatten<F> f)
        {
            TheTable = f;
        }
        public TableJoinType TableJoin { get; set; }

        public SqlPatten<F> TheTable { get; set; }

        public JoinedTable Join<C>(JoinTable<C> SecondTable)
        {
            var joinedTable = new JoinedTable(2);
            var list = TheTable.SelectPattens;
            list.AddRange(SecondTable.TheTable.SelectPattens);
            joinedTable.SelectList = list;
            joinedTable.sqlString = "";
            return joinedTable;
        }

    }

    public class JoinedTable
    {
        public JoinedTable() { }
        public JoinedTable(int num)
        {
            _joinedNum = num;
        }
        private int _joinedNum { get; set; } = 0;
        public List<string> SelectList { get; set; }

        public string sqlString { get; set; }

        public JoinedTable Join<C>(JoinTable<C> SecondTable)
        {
            _joinedNum++;


            return this;
        }

    }

}
