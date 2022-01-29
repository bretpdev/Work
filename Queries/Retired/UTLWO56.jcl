#UTLWO56.jcl 
#
#set environment variables

 . /sas/whse/copy_lib/sasprofile
 . /sas/whse/copy_lib/pathprofile.dblgs

# delete any existing report files used for testing

if test -a ${reportdir}/ULWO56.LWO56R1
then
rm ${reportdir}/ULWO56.LWO56R1
fi
if test -a ${reportdir}/ULWO56.LWO56R2
then
rm ${reportdir}/ULWO56.LWO56R2
fi
if test -a ${reportdir}/ULWO56.LWO56RZ
then
rm ${reportdir}/ULWO56.LWO56RZ
fi

# run the program

sas ${codedir}/UTLWO56.sas -log ${reportdir}/ULWO56.LWO56R1  -mautosource
