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

        public JoinedTable Join<C>(JoinTable<C> SecondTable,TableJoinType joinType)
        {
            var joinedTable = new JoinedTable();
            var list = TheTable.SelectAliaPattens;
            list.AddRange(SecondTable.TheTable.SelectAliaPattens);
            joinedTable.SelectList = list;

            string MatchName = "";
            TableJoinHelper.CheckTableJoinVaild<F,C>(TheTable.SelectAliaPattens, 
                SecondTable.TheTable.SelectAliaPattens,out MatchName);

            joinedTable.sqlString = TableJoinHelper.TableJoinSqlString(TheTable.ToSqlString(),
                SecondTable.TheTable.ToSqlString(),TableJoin,MatchName);

            return joinedTable;
        }

    }

    public class JoinedTable
    {
        public JoinedTable() { }

        public List<string> SelectList { get; set; }

        public string sqlString { get; set; }

        public JoinedTable Join<C>(JoinTable<C> SecondTable,TableJoinType joinType)
        {
            string MatchName = "";
            TableJoinHelper.CheckTableJoinVaild<JoinedTable, C>(this.SelectList,
                SecondTable.TheTable.SelectAliaPattens, out MatchName);

            var list = SelectList;
            list.AddRange(SecondTable.TheTable.SelectAliaPattens);
            this.SelectList = list;

            var baseSqlString = string.Format("select * from ({0})", sqlString);

            this.sqlString = TableJoinHelper.TableJoinSqlString(baseSqlString,
                SecondTable.TheTable.ToSqlString(), joinType, MatchName);

            return this;
        }

    }

}
