#UTLWI04.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWI04.LWI04R1
then
rm ${reportdir}/ULWI04.LWI04R1
fi
if test -a ${reportdir}/ULWI04.LWI04R2
then
rm ${reportdir}/ULWI04.LWI04R2
fi
if test -a ${reportdir}/ULWI04.LWI04RZ
then
rm ${reportdir}/ULWI04.LWI04RZ
fi

# run the program

sas ${codedir}/UTLWI04.sas -log ${reportdir}/ULWI04.LWI04R1  -mautosource
