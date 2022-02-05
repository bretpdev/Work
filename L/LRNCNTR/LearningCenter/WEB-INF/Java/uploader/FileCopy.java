package uploader;

import java.io.*;
import java.util.*;

public class FileCopy {
	public static void copyFile(String origf, String newf) throws Exception {
		FileInputStream fis = new FileInputStream(new File(origf));
		FileOutputStream fos = new FileOutputStream(new File(newf));
		try {
			byte[] buf = new byte[1024];
			int i = 0;
			while ((i = fis.read(buf)) != -1) { fos.write(buf, 0, i); }
		}
		catch (Exception e) {
			throw;
		}
		finally {
			if (fis != null) fis.close();
			if (fos != null) fos.close();
		}
		File del = new File(origf);
		del.delete();
	}
}
