#UTLWM25.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWM25.LWM25R1
then
rm ${reportdir}/ULWM25.LWM25R1
fi
if test -a ${reportdir}/ULWM25.LWM25R2
then
rm ${reportdir}/ULWM25.LWM25R2
fi
if test -a ${reportdir}/ULWM25.LWM25RZ
then
rm ${reportdir}/ULWM25.LWM25RZ
fi

# run the program

sas ${codedir}/UTLWM25.sas -log ${reportdir}/ULWM25.LWM25R1  -mautosource
