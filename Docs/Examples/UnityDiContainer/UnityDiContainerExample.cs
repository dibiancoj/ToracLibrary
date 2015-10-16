        static void Main(string[] args)
        {
            using (var container = new UnityContainer())
            {
                //ContainerControlledLifetimeManager = singleton
                //TransientLifetimeManager = new instance each time

                //simple example
                container.RegisterType<IDataProvider, SimpleGuruDataProvider>("Simple", new ContainerControlledLifetimeManager());

                var simpleDP = container.Resolve<IDataProvider>("Simple");
                var simpleDPRecords = simpleDP.GetRecords();

                //******************************************************
                //constructor parameter example
                container.RegisterType<IDataProvider, CtorParameterGuruDataProvider>("CtorParameter", new TransientLifetimeManager(), new InjectionConstructor(24));

                var ctorParameterDP = container.Resolve<IDataProvider>("CtorParameter");
                var ctorParameterDPRecords = ctorParameterDP.GetRecords();

                //******************************************************
                //generic type
                container.RegisterType(typeof(IDataProvider), typeof(GenericTypeGuruDataProvider<string>), "GenericTypeParameter", new InjectionConstructor(24));

                var genericTypeParameterDP = container.Resolve<IDataProvider>("GenericTypeParameter");
                var genericTypeDPRecords = genericTypeParameterDP.GetRecords();

                //******************************************************
                //generic type as parameter
                container.RegisterType(typeof(IDataProvider), typeof(GenericTypeWithConstuctorGuruDataProvider<string>), "GenericTypeParameterInConstructor", new InjectionConstructor("test", 24));

                var genericTypeParameterInConstructorDP = container.Resolve<IDataProvider>("GenericTypeParameterInConstructor");
                var genericTypeInConstructorDPRecords = genericTypeParameterInConstructorDP.GetRecords();
                //******************************************************

                //func to determine which item to resolvez
                container.RegisterType<Func<string, IDataProvider>>(
                    new InjectionFactory(x =>
                    new Func<string, IDataProvider>(name => x.Resolve<IDataProvider>(name))));
            }
        }

        public interface IDataProvider
        {
            IEnumerable<string> GetRecords();
        }

        public class SimpleGuruDataProvider : IDataProvider
        {
            public IEnumerable<string> GetRecords()
            {
                return new string[] { "1", "2", "3" };
            }
        }

        public class CtorParameterGuruDataProvider : IDataProvider
        {
            public CtorParameterGuruDataProvider(int i)
            {
                Id = i;
            }

            public int Id { get; set; }

            public IEnumerable<string> GetRecords()
            {
                return new string[] { "4", "5", "6" };
            }
        }

        public class GenericTypeGuruDataProvider<TContext> : IDataProvider
        {
            public GenericTypeGuruDataProvider(int i)
            {
                Id = i;
            }

            public int Id { get; set; }

            public IEnumerable<string> GetRecords()
            {
                return new string[] { "4", "5", "6" };
            }

        }

        public class GenericTypeWithConstuctorGuruDataProvider<TContext> : IDataProvider
        {
            public GenericTypeWithConstuctorGuruDataProvider(TContext t, int i)
            {
                Id = i;
                T = t;
            }

            public int Id { get; set; }
            public TContext T { get; set; }

            public IEnumerable<string> GetRecords()
            {
                return new string[] { "4", "5", "6" };
            }

        }