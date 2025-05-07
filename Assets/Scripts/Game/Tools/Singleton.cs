using Mirror;

public class SingletonNetBehaviour<T> : NetworkBehaviour where T : SingletonNetBehaviour<T> 
{
    private void Start()
    {
        if (m_instance == null)
            m_instance = (T)this;
        else
            if(m_instance != this)
                Destroy(gameObject);
    }
    private static T m_instance;
    public static T instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindAnyObjectByType<T>();
            }
            return m_instance;
        }
    }
}