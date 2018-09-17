using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Lesson1
{
    class PhoneBook : IEnumerable
    {
        private string strConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\davom\source\repos\LINQ\Lesson1\DBPBookmdf.mdf;Integrated Security=True";
        public PhoneBook Next { get; private set; }
        private PhoneBook Current { get; set; }
        public Contact Value { get; set; }
        private bool FirstTime { get; set; } = true;
        public int Count { get; private set; } = 0;

        public Contact this[int index]
        {
            get { return GetValue(index); }
            set { SetValue(index, value); }
        }

        private Contact GetValue(int index)
        {
            Current = this;
            int i = 0;
            while (i != index)
            {
                Current = Current.Next;
                i++;
            }
            return Current.Value;
        }

        private void SetValue(int index, Contact value)
        {
            Current = this;
            int i = 0;
            while (i != index)
            {
                Current = Current.Next;
                i++;
            }
            Current.Value = value;
        }


        public PhoneBook()
        {

        }

        public PhoneBook(Contact value)
        {
            Value = value;
            FirstTime = false;
            Count++;
        }

        //Clear
        public void Clear()
        {
            this.Next = null;
            this.Value = null;
            Count = 0;

            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                connection.Open();
                string countTable = @"SELECT COUNT(*) FROM [ContactList]";
                SqlCommand c = new SqlCommand(countTable, connection);
                int count = (int)c.ExecuteScalar();
                for (int i = 0; i < count; i++)
                {
                    SqlCommand command = new SqlCommand($"DELETE FROM [ContactList] WHERE [Id] = {i + 1}", connection);
                    command.ExecuteNonQuery();
                }
            }

            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("DBCC CHECKIDENT ('[ContactList]', RESEED, 0);", connection);
                command.ExecuteNonQuery();
            }
        }

        private int numOfInserted = 0;
        //Add
        public void Add(Contact contact)
        {

            if (FirstTime)
            {
                Value = contact;
                FirstTime = false;
                Count++;

                string CommandInsert = $@"INSERT INTO ContactList ([Name], [SurName], [Email], [PNumber]) VALUES 
                                        ('{Value.Name}', '{Value.SurName}', '{Value.Email}', '{Value.PNumber}')";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(CommandInsert, connection);
                    numOfInserted += command.ExecuteNonQuery();
                }
            }
            else if (Next == null)
            {
                Next = new PhoneBook(contact);
                Current = Next;
                Count++;

                string CommandInsert = $@"INSERT INTO ContactList ([Name], [SurName], [Email], [PNumber]) VALUES 
                                        ('{Current.Value.Name}', '{Current.Value.SurName}', '{Current.Value.Email}', '{Current.Value.PNumber}')";

                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(CommandInsert, connection);
                    numOfInserted += command.ExecuteNonQuery();
                }
            }
            else
            {
                Current.Next = new PhoneBook(contact);
                Current = Current.Next;
                Count++;
                string CommandInsert = $@"INSERT INTO ContactList ([Name], [SurName], [Email], [PNumber]) VALUES 
                                        ('{Current.Value.Name}', '{Current.Value.SurName}', '{Current.Value.Email}', '{Current.Value.PNumber}')";


                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(CommandInsert, connection);
                    numOfInserted += command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }



        private struct Enumerator : IEnumerator
        {
            private PhoneBook Pb { get; set; }

            public object Current { get; set; }

            public Enumerator(PhoneBook pb)
            {
                Current = null;
                Pb = pb;
            }

            public bool MoveNext()
            {
                if (Pb != null)
                {
                    Current = Pb.Value;
                    Pb = Pb.Next;
                    return true;
                }
                else
                    return false;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
