namespace EWPF_UnitTests.MVVM.BaseViewModel
{
    /// <summary>
    /// A testable view model created for running unit test against.
    /// </summary>
    public class TestableViewModel : EWPF.MVVM.BaseViewModel
    {
        #region Events



        #endregion

        #region Fields

        #region Model



        #endregion

        #region Commands



        #endregion

        #region Other

        private const string cm_TEST_PROPERTY_NAME = "TestProperty";
        private object m_TestProperty;

        #endregion

        #endregion

        #region Constructors



        #endregion

        #region Methods

        #region Command Executions



        #endregion

        #region Other



        #endregion

        #endregion

        #region Properties

        #region Model

        /// <summary>
        /// Testable property.
        /// </summary>
        public object TestProperty
        {
            get { return m_TestProperty; }
            set
            {
                HasPropertyChanged = false;
                if (Equals(m_TestProperty, value)) return;
                m_TestProperty = value;
                HasPropertyChanged = true;
            }
        }

        #endregion

        #region Commands



        #endregion

        #region Other

        /// <summary>
        /// Indicates weather <see cref="TestProperty"/>'s value has changed.
        /// </summary>
        public bool HasPropertyChanged { get; set; }

        #endregion

        #endregion
    }
}