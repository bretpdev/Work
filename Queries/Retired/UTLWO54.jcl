#UTLWO54.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO54.LWO54R1
then
rm ${reportdir}/ULWO54.LWO54R1
fi
if test -a ${reportdir}/ULWO54.LWO54R2
then
rm ${reportdir}/ULWO54.LWO54R2
fi
if test -a ${reportdir}/ULWO54.LWO54RZ
then
rm ${reportdir}/ULWO54.LWO54RZ
fi

# run the program

sas ${codedir}/UTLWO54.sas -log ${reportdir}/ULWO54.LWO54R1  -mautosource
