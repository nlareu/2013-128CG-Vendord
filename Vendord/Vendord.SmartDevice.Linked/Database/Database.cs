﻿[module:
    System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules", "*",
        Justification = "Reviewed. Suppression of all documentation rules is OK here.")]

namespace Vendord.SmartDevice.Linked
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlServerCe;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Represents an business object.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The order system works in a distributed environment, where users can INSERT orders on sereral machines (nodes). 
    /// Ergo, the PRIMARY KEY must be unique across all nodes and must not be reused. Options:
    /// 1. Auto-increment (Identity) Columns. High probability of PK collision if INSERT operations occur in multiple nodes. 
    /// 2. GUIDs. Essentially no probability of PK collisions but can lead to large and fragmented clustered indexes.
    /// 3. Keys that Include a Node Identifier. Add a node identifier to the autogenerated identity value.
    /// 4. Natural Keys. Use column values such as Social Insurance Numbers or Upc codes that must be unique already.
    /// 5. Online insertion.
    /// </para>
    /// <seealso>
    /// Selecting PKs for SYNC http://msdn.microsoft.com/en-us/library/bb726011.aspx
    /// GUID Generation http://social.msdn.microsoft.com/Forums/sqlserver/en-US/af52661f-7eb5-4c73-87e8-2d9ad195e112/algorithm-to-generate-guids-in-sql-server?forum=transactsql
    /// </seealso>
    /// </remarks>       
    public class Database
    {
        private const string SqlCeConnectionStringTemplate = @"Data Source={0};Persist Security Info=False;";

        private DbQueryExecutor queryExecutor;
        private DbSchemaBuilder schemaBuilder;

        private List<Product> _products;
        private List<Order> _order;
        private List<OrderProduct> _orderProducts;
        private List<Vendor> _vendors;
        private List<Department> _departments;        
        private readonly string _connectionString;

        public Database() : this(Constants.VendordMainDatabaseFullPath)
        {            
        }

        public Database(string fullPath)
        {
            this._connectionString = GenerateSqlCeConnString(fullPath);
            this.queryExecutor = new DbQueryExecutor(this._connectionString);

            this.schemaBuilder = new DbSchemaBuilder(queryExecutor);
            this.schemaBuilder.CreateCeDb(fullPath, this._connectionString);
            this.schemaBuilder.CreateTables();                        
        }

        public string ConnectionString 
        {
            get
            {
                return _connectionString;
            }
        }

        public List<Order> Orders
        {
            get
            {
                SqlCeDataReader reader;
                System.Data.SqlServerCe.SqlCeCommand command;

                if (this._order == null)
                {
                    this._order = new List<Order>();
                    using (SqlCeConnection conn = new SqlCeConnection(this._connectionString))
                    {
                        conn.Open();
                        command = new SqlCeCommand(@"SELECT * FROM tblOrder WHERE IsInTrash IS NULL OR IsInTrash = 0", conn);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Order item = new Order()
                            {
                                Id = new Guid(reader["Id"].ToString()),
                                Name = Convert.ToString(reader["Name"])
                            };
                            this._order.Add(item);
                        }
                    }
                }

                return this._order;
            }
        }

        public List<Product> Products
        {
            get
            {
                SqlCeDataReader reader;
                SqlCeCommand command;

                if (this._products == null)
                {
                    this._products = new List<Product>();
                    using (SqlCeConnection conn = new SqlCeConnection(this._connectionString))
                    {
                        conn.Open();
                        command = new SqlCeCommand(@"SELECT * FROM tblProduct", conn);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Product item = new Product()
                            {
                                Name = Convert.ToString(reader["Name"]),
                                Upc = Convert.ToString(reader["Upc"]),
                                Price = Convert.ToDecimal(reader["Price"]),
                                DepartmentId = new Guid(reader["DepartmentId"].ToString()),
                                VendorId = new Guid(reader["VendorId"].ToString())
                            };
                            this._products.Add(item);
                        }
                    }
                }

                return this._products;
            }
        }

        public List<OrderProduct> OrderProducts
        {
            get
            {
                if (this._orderProducts == null)
                {
                    SqlCeDataReader reader;
                    SqlCeCommand command;

                    this._orderProducts = new List<OrderProduct>();
                    using (SqlCeConnection conn = new SqlCeConnection(this._connectionString))
                    {
                        conn.Open();
                        command = new SqlCeCommand(@"SELECT * FROM tblOrderProduct WHERE IsInTrash IS NULL OR IsInTrash = 0", conn);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            OrderProduct item = new OrderProduct()
                            {
                                ProductUPC = reader["ProductUPC"].ToString(),
                                OrderID = new Guid(reader["OrderID"].ToString()),
                                CasesToOrder = Convert.ToInt32(reader["CasesToOrder"])
                            };
                            this._orderProducts.Add(item);
                        }
                    }
                }

                return this._orderProducts;
            }
        }

        public List<Vendor> Vendors
        {
            get
            {
                SqlCeDataReader reader;
                System.Data.SqlServerCe.SqlCeCommand command;

                if (this._vendors == null)
                {
                    this._vendors = new List<Vendor>();
                    using (SqlCeConnection conn = new SqlCeConnection(this._connectionString))
                    {
                        conn.Open();
                        command = new SqlCeCommand(@"SELECT * FROM tblVendor WHERE IsInTrash IS NULL OR IsInTrash = 0", conn);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Vendor item = new Vendor()
                            {
                                Id = new Guid(reader["Id"].ToString()),
                                Name = Convert.ToString(reader["Name"])
                            };
                            this._vendors.Add(item);
                        }
                    }
                }

                return this._vendors;
            }
        }

        public List<Department> Departments
        {
            get
            {
                SqlCeDataReader reader;
                System.Data.SqlServerCe.SqlCeCommand command;

                if (this._departments == null)
                {
                    this._departments = new List<Department>();
                    using (SqlCeConnection conn = new SqlCeConnection(this._connectionString))
                    {
                        conn.Open();
                        command = new SqlCeCommand(@"SELECT * FROM tblDepartment WHERE IsInTrash IS NULL OR IsInTrash = 0", conn);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Department item = new Department()
                            {
                                Id = new Guid(reader["Id"].ToString()),
                                Name = Convert.ToString(reader["Name"])
                            };
                            this._departments.Add(item);
                        }
                    }
                }

                return this._departments;
            }
        }

        public static string GenerateSqlCeConnString(string databaseFullPath)
        {
            string sqlCeConnString;
            sqlCeConnString = string.Format(SqlCeConnectionStringTemplate, databaseFullPath);
            return sqlCeConnString;
        }

        public void EmptyTrash()
        {
            (new Order()).EmptyTrash(this.queryExecutor);
            (new Product()).EmptyTrash(this.queryExecutor);
            (new OrderProduct()).EmptyTrash(this.queryExecutor);
        } 
    }
}