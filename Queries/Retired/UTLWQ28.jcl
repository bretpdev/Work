#UTLWQ28.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWQ28.LWQ28R1
then
rm ${reportdir}/ULWQ28.LWQ28R1
fi
if test -a ${reportdir}/ULWQ28.LWQ28R2
then
rm ${reportdir}/ULWQ28.LWQ28R2
fi
if test -a ${reportdir}/ULWQ28.LWQ28RZ
then
rm ${reportdir}/ULWQ28.LWQ28RZ
fi

# run the program

sas ${codedir}/UTLWQ28.sas -log ${reportdir}/ULWQ28.LWQ28R1  -mautosource
