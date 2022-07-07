package org.csystem.samples.makeupperc.androidclient;

import android.os.Bundle;
import android.os.Handler;
import android.os.Looper;
import android.os.Message;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import org.csystem.netx.SocketUtilX;

import java.io.IOException;
import java.lang.ref.WeakReference;
import java.net.Socket;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class MainActivity extends AppCompatActivity {
    private Socket m_socket;
    private ExecutorService m_threadPool;
    private Handler m_networkHandler;
    private EditText m_editTextText;


    private static class NetworkHandler extends Handler {
        private final WeakReference<MainActivity> m_mainActivityWeakReference;

        public NetworkHandler(MainActivity mainActivity)
        {
            super(Looper.myLooper());
            m_mainActivityWeakReference = new WeakReference<>(mainActivity);
        }

        @Override
        public void handleMessage(@NonNull Message msg)
        {
            if (msg.what == -1)
                Toast.makeText(m_mainActivityWeakReference.get(), msg.obj.toString(), Toast.LENGTH_LONG).show();
            else if (msg.what == 0)
                Toast.makeText(m_mainActivityWeakReference.get(), msg.obj.toString(), Toast.LENGTH_LONG).show();

            super.handleMessage(msg);
        }
    }

    private void initThreadPool()
    {
        m_threadPool = Executors.newSingleThreadExecutor();
    }

    private void initSocket()
    {
        m_threadPool.submit(() -> {
            try {
                m_socket = new Socket("192.168.1.200", 5120);
            }
            catch (IOException ex) {
                m_networkHandler.sendMessage(m_networkHandler.obtainMessage(-1, ex.getMessage()));
            }
        });
    }

    private void init()
    {
        m_networkHandler = new NetworkHandler(this);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        init();
    }

    @Override
    protected void onResume()
    {
        initThreadPool();
        initSocket();
        super.onResume();
    }

    @Override
    protected void onPause()
    {
        try {
            m_threadPool.submit(() -> {
                try {
                    //m_socket.close();
                    if (m_socket != null)
                        SocketUtilX.sendString(m_socket, "quit");
                }
                catch (Throwable ex) {
                    m_networkHandler.sendMessage(m_networkHandler.obtainMessage(-1, ex.getMessage()));
                }
            });

            m_threadPool.shutdown();
        }
        catch (Throwable ex) {
            Toast.makeText(this, ex.getMessage(), Toast.LENGTH_LONG).show();
        }
        super.onPause();
    }

    public void onSendButtonClicked(View view)
    {
        m_threadPool.submit(() -> {
            try {
                SocketUtilX.sendString(m_socket, "ankara");
                String result = SocketUtilX.receiveString(m_socket);

                m_networkHandler.sendMessage(m_networkHandler.obtainMessage(0, result));
            }
            catch (Throwable ex) {
                m_networkHandler.sendMessage(m_networkHandler.obtainMessage(-1, ex.getMessage()));
            }
        });
    }
}