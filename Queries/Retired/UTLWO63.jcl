#UTLWO63.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO63.LWO63R1
then
rm ${reportdir}/ULWO63.LWO63R1
fi
if test -a ${reportdir}/ULWO63.LWO63R2
then
rm ${reportdir}/ULWO63.LWO63R2
fi
if test -a ${reportdir}/ULWO63.LWO63RZ
then
rm ${reportdir}/ULWO63.LWO63RZ
fi

# run the program

sas ${codedir}/UTLWO63.sas -log ${reportdir}/ULWO63.LWO63R1  -mautosource
